using Citadels.Client.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Resources;
using Telegram.Bot;
using Telegram.Bot.Polling;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets("6501faf3-a56a-49ca-9573-5e5bf8e73409")
    .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton(new ResourceManager(typeof(Citadels.Client.Telegram.Resources.Strings)))
    .AddSingleton<IUpdateHandler, TelegramUpdateHandler>()
    .BuildServiceProvider();

var cancellationTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += (_, _) => cancellationTokenSource.Cancel();

var token = configuration["Telegram:BotToken"]!;
var telegramBot = new TelegramBotClient(token);
telegramBot.StartReceiving(serviceProvider.GetRequiredService<IUpdateHandler>(), cancellationToken: cancellationTokenSource.Token);

Console.ReadKey(); //don't know yet how to do better