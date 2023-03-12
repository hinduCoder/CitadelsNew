namespace Citadels.Client.Telegram.KeyboardTemplates;

public class ButtonFactory<TState>
{
    public List<IInlineKeyboardButtonTemplate<TState>> Buttons { get; set; } = new();
    public ButtonFactory<TState> Button(string text, string callbackData, Func<TState, bool>? showCondition = null)
    {
        Buttons.Add(new InlineKeyboardButtonTemplate<TState>(text, callbackData, showCondition));
        return this;
    }

    public ButtonFactory<TState> Link(string text, string url, Func<TState, bool>? showCondition = null)
    {
        Buttons.Add(new InlineKeyboardLinkButtonTemplate<TState>(text, url, showCondition));
        return this;
    }
}
