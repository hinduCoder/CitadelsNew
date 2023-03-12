using Telegram.Bot.Types.ReplyMarkups;

namespace Citadels.Client.Telegram.KeyboardTemplates;

public interface IInlineKeyboardButtonTemplate<TState>
{
    InlineKeyboardButton Button { get; }

    bool Show(TState state);
}
