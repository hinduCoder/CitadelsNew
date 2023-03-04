using Citadels.Client.Telegram;
using Citadels.Client.Telegram.CommandHandlers;
using Citadels.Client.Telegram.Commands;
using Citadels.Client.Telegram.Commands.Handlers;
using Citadels.Client.Telegram.Resources;
using Citadels.Client.Telegram.TelegramExnteions;
using Citadels.Client.Telegram.Templates;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Resources;
using Telegram.Bot;
using Telegram.Bot.Polling;
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
    .AddSingleton<IStringsProvider>(new StringProvider(new ResourceManager(typeof(Strings))))
    .AddSingleton<IKeyboardLocalizator, KeyboardLocalizator>()
    .AddSingleton<HandlerbarsInitializer>()
    .Scan(x => x.FromCallingAssembly()
                .AddClasses(x => x.AssignableTo<ICommandHandler>())
                .AsSelf()
                .WithScopedLifetime())
    .AddSingleton<ILogger>(loggerConfiguration.CreateLogger())
    .AddSingleton<ITelegramBotClient>(serviceProvider =>
    {
        var token = serviceProvider.GetRequiredService<IConfiguration>()["Telegram:BotToken"]!;
        return new TelegramBotClient(token);
    })
    .AddSingleton<TelegramBotSettingsInitializer>()
    .AddDbContext<TelegramClientDbContext>((serviceProvider, options) 
        => options.UseNpgsql(serviceProvider
            .GetRequiredService<IConfiguration>()
            .GetConnectionString("Postgres")))
    .AddScoped(serviceProvider => GrpcChannel.ForAddress(serviceProvider.GetRequiredService<IConfiguration>()["Api:Host"]!))
    .AddScoped(serviceProvider => new Citadels.Api.Citadels.CitadelsClient(serviceProvider.GetRequiredService<GrpcChannel>()))
    .BuildServiceProvider();

serviceProvider.GetRequiredService<HandlerbarsInitializer>().Initialize();

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

var customChatTemplate = configuration.GetValue<bool>("Settings:CustomInGameChatTemplate");
var router = new RouterBuilder()
    .Map<RegistrationHandler>(update => update.Message is { Text: not null, Chat.Type: ChatType.Private } message && message.Text.StartsWith("/start") || update.CallbackQuery is { Data: CallbackData.CancelRegistration })
    .Map<DraftHandler>(update => update is { CallbackQuery.Data: not null })
    .Map<CustomInGameChatHandler>(update => customChatTemplate && update.Message is not null)
    .Map<StandardInGameChatHandler>(update => !customChatTemplate && update.Message is not null)
    .Build();

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
        using var scope = serviceProvider.CreateScope();
        var updateHandler = router.GetHandler(update, scope.ServiceProvider);
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
    new ReceiverOptions { ThrowPendingUpdates = true }, 
    cancellationToken: cancellationTokenSource.Token);

logger.Information("Gracefull shutting down");
await Task.Delay(5000);