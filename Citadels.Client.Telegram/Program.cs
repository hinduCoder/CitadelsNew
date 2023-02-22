using Citadels.Client.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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
    .AddSingleton<IUpdateHandler, TelegramUpdateHandler>()
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

var botInitializer = serviceProvider.GetRequiredService<TelegramBotSettingsInitializer>();
await botInitializer.SetCommands();

var telegramBot = serviceProvider.GetRequiredService<ITelegramBotClient>();
telegramBot.StartReceiving(serviceProvider.GetRequiredService<IUpdateHandler>(), cancellationToken: cancellationTokenSource.Token);

try
{
    await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token);
} catch (TaskCanceledException)
{
}

Console.WriteLine("Gracefull shutting down");
await Task.Delay(5000);