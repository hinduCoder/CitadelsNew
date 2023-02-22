using Citadels.Client.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Resources;
using Telegram.Bot;
using Telegram.Bot.Polling;

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
    .AddSingleton(new ResourceManager(typeof(Citadels.Client.Telegram.Resources.Strings)))
    .AddScoped<IUpdateHandler, TelegramUpdateHandler>()
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
    .BuildServiceProvider();

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

var telegramBot = serviceProvider.GetRequiredService<ITelegramBotClient>();
await telegramBot.ReceiveAsync(async (t, u, ct) =>
{
    using var scope = serviceProvider.CreateScope();
    var updateHandler = scope.ServiceProvider.GetRequiredService<IUpdateHandler>();
    await updateHandler.HandleUpdateAsync(t, u, ct);
}, (_, ex, _) =>
{
    logger.Error(ex, "Telegram update handler error");
    return Task.CompletedTask;
}, cancellationToken: cancellationTokenSource.Token);

logger.Information("Gracefull shutting down");
await Task.Delay(5000);