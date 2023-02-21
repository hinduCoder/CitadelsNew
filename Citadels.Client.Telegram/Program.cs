using Citadels.Client.Telegram.Resources;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Resources;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var resourceManager = new ResourceManager(typeof(Strings));
Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

Console.WriteLine(resourceManager.GetString("Welcome"));
