using Citadels.Client.Telegram.Resources;
using Telegram.Bot.Types.ReplyMarkups;

namespace Citadels.Client.Telegram.TelegramExnteions;

public class KeyboardLocalizator : IKeyboardLocalizator
{
    private IStringsProvider _stringsProvider;

    public KeyboardLocalizator(IStringsProvider stringsProvider)
    {
        _stringsProvider = stringsProvider;
    }

    public T Localize<T>(T button, string? languageCode) where T : IKeyboardButton
    {
        var newText = _stringsProvider.Get(button.Text, languageCode);
        if (newText is null)
        {
            return button;
        }
        button.Text = newText;
        if (button is InlineKeyboardButton { Url: not null } urlButton && !urlButton.Url.StartsWith("http"))
        {
            urlButton.Url = _stringsProvider.Get(urlButton.Url, languageCode);
        }
        return button;
    }
}
