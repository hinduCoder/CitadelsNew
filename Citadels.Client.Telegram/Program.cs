using Citadels.Client.Telegram;
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

var serviceProvider = new ServiceCollection()
    .AddSingleton(configuration)
    .AddSingleton(new ResourceManager(typeof(Citadels.Client.Telegram.Resources.Strings)))
    .AddSingleton<IUpdateHandler, TelegramUpdateHandler>()
    .AddSingleton<ILogger>(loggerConfiguration.CreateLogger())
    .BuildServiceProvider();

var cancellationTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    cancellationTokenSource.Cancel();
    e.Cancel = true;
};

var token = configuration["Telegram:BotToken"]!;
var telegramBot = new TelegramBotClient(token);
telegramBot.StartReceiving(serviceProvider.GetRequiredService<IUpdateHandler>(), cancellationToken: cancellationTokenSource.Token);

try
{
    await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token);
} catch (TaskCanceledException)
{
}

Console.WriteLine("Gracefull shutting down");
await Task.Delay(5000);