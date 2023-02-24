using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Citadels.Client.Telegram.TelegramExnteions;

public static class ITelegramBotClientExtensions
{
    public static async Task<Message> SendOrEditMessageAsync(this ITelegramBotClient telegramBotClient,
        ChatId chatId, int? messageId, string text, ParseMode parseMode, InlineKeyboardMarkup replyMarkup,
        CancellationToken cancellationToken = default)
    {
        if (messageId.HasValue)
        {
            return await telegramBotClient.EditMessageTextAsync(chatId, messageId.Value, text, parseMode,
                replyMarkup: replyMarkup, cancellationToken: cancellationToken);
        }
        return await telegramBotClient.SendTextMessageAsync(chatId, text, parseMode,
            replyMarkup: replyMarkup, cancellationToken: cancellationToken);

    }
}
