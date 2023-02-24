using Citadels.Client.Telegram;
using Citadels.Client.Telegram.CommandHandlers;
using Citadels.Client.Telegram.Resources;
using Citadels.Client.Telegram.TelegramExnteions;
using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Resources;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets("6501faf3-a56a-49ca-9573-5e5bf8e73409")
    .Build();

var loggerConfiguration = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .WriteTo.Console();

using var serviceProvider = new ServiceCollection()
    .AddSingleton(configuration)
    .AddSingleton<IStringsProvider>(new StringProvider(new ResourceManager(typeof(Citadels.Client.Telegram.Resources.Strings))))
    .AddSingleton<IKeyboardLocalizator, KeyboardLocalizator>()
    .Scan(x => x.FromCallingAssembly()
                .AddClasses(x => x.AssignableTo<ICommandHandler>())
                .As<ICommandHandler>()
                .WithScopedLifetime())
    .AddSingleton<ILogger>(loggerConfiguration.CreateLogger())
    .AddSingleton<ITelegramBotClient>(serviceProvider =>
    {
        var token = serviceProvider.GetRequiredService<IConfiguration>()["Telegram:BotToken"]!;
        return new TelegramBotClient(token);
    })
    .AddSingleton<TelegramBotSettingsInitializer>()
    .AddDbContextFactory<TelegramClientDbContext>((serviceProvider, options) 
        => options.UseNpgsql(serviceProvider
            .GetRequiredService<IConfiguration>()
            .GetConnectionString("Postgres")))
    .BuildServiceProvider();

var stringProvider = serviceProvider.GetRequiredService<IStringsProvider>();
Handlebars.RegisterHelper("res", (writer, context, args) =>
{
    writer.WriteSafeString(stringProvider.Get(args.At<string>(0), context.GetValue<string>("Language")));
});

var cancellationTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    cancellationTokenSource.Cancel();
    e.Cancel = true;
};

var logger = serviceProvider.GetRequiredService<ILogger>();

logger.Information("Started");

var botInitializer = serviceProvider.GetRequiredService<TelegramBotSettingsInitializer>();
await botInitializer.SetCommands();

logger.Information("Bot initialization done");
logger.Information("Starting listening");

var commandHandler = serviceProvider.GetServices<ICommandHandler>().OrderByDescending(x => x.Order).ToList();

var telegramBot = serviceProvider.GetRequiredService<ITelegramBotClient>();
await telegramBot.ReceiveAsync(async (_, update, cancellationToken) =>
    {
        switch (update)
        {
            case { Message: not null and var message }:
                logger.Information("Message {Message} ({Lang})", message.Text, update.Message.From!.LanguageCode);
                break;
            case { CallbackQuery: not null and var query }:
                logger.Information("Callback query {Data}", query.Data);
                break;
        }

        var updateHandler = commandHandler.FirstOrDefault(x => x.CanHandle(update));
        if (updateHandler is null)
        {
            return;
        }
        await updateHandler.Handle(update, cancellationToken);
    }, (_, ex, _) =>
    {
        logger.Error(ex, "Telegram update handler error");
        return Task.CompletedTask;
    }, 
    new ReceiverOptions { ThrowPendingUpdates = false }, 
    cancellationToken: cancellationTokenSource.Token);

logger.Information("Gracefull shutting down");
await Task.Delay(5000);