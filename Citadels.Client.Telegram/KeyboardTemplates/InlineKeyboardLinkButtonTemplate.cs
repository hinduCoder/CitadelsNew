using Telegram.Bot.Types.ReplyMarkups;

namespace Citadels.Client.Telegram.KeyboardTemplates;

public class InlineKeyboardLinkButtonTemplate<TState> : IInlineKeyboardButtonTemplate<TState>
{
    private readonly string _text;
    private readonly string _url;
    private readonly Func<TState, bool>? _showCondition;
    public InlineKeyboardButton Button => InlineKeyboardButton.WithUrl(_text, _url);

    public InlineKeyboardLinkButtonTemplate(string text, string url, Func<TState, bool>? showCondition = null)
    {
        _text = text;
        _url = url;
        _showCondition = showCondition;
    }

    public bool Show(TState state) => _showCondition?.Invoke(state) ?? true;
}
