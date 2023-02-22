using Citadels.Client.Telegram.Resources;
using Serilog;
using System.Globalization;
using System.Resources;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Citadels.Client.Telegram;

internal class TelegramUpdateHandler : IUpdateHandler
{
    private readonly ResourceManager _resourceManager;
    private readonly ILogger _logger;

    public TelegramUpdateHandler(ResourceManager resourceManager, ILogger logger)
    {
        _resourceManager = resourceManager;
        _logger = logger;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.Error(exception, "Telegram update handler error");
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is null)
        {
            return;
        }

        var languageCode = update.Message.From!.LanguageCode;
        _logger.Information("Message {Message} ({Lang})", update.Message.Text, languageCode);
        var culture = CultureInfo.CreateSpecificCulture(languageCode ?? "en-US");
        await botClient.SendTextMessageAsync(update.Message.Chat.Id, _resourceManager.GetString("Welcome", culture)!, cancellationToken: cancellationToken);
    }
}
