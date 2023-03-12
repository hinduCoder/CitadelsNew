using Telegram.Bot.Types.ReplyMarkups;

namespace Citadels.Client.Telegram.KeyboardTemplates;

public class InlineKeyboardButtonTemplate<TState> : IInlineKeyboardButtonTemplate<TState>
{
    private readonly string _text;
    private readonly string _callbackData;
    private readonly Func<TState, bool>? _showCondition;
    public InlineKeyboardButton Button => InlineKeyboardButton.WithCallbackData(_text, _callbackData);

    public InlineKeyboardButtonTemplate(string text, string callbackData, Func<TState, bool>? showCondition = null)
    {
        _text = text;
        _callbackData = callbackData;
        _showCondition = showCondition;
    }

    public bool Show(TState state) => _showCondition?.Invoke(state) ?? true;
}
