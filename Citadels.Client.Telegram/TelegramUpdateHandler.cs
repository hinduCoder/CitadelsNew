using Citadels.Client.Telegram.Resources;
using System.Globalization;
using System.Resources;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Citadels.Client.Telegram;

internal class TelegramUpdateHandler : IUpdateHandler
{
    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is null)
        {
            return;
        }
        var resourceManager = new ResourceManager(typeof(Strings));
        Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(update.Message.From!.LanguageCode ?? "en-US");
        await botClient.SendTextMessageAsync(update.Message.Chat.Id, resourceManager.GetString("Welcome")!);
    }
}
