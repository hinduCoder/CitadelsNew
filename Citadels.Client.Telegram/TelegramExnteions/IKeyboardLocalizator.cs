using Telegram.Bot.Types.ReplyMarkups;

namespace Citadels.Client.Telegram.TelegramExnteions;
public interface IKeyboardLocalizator
{
    T Localize<T>(T button, string? languageCode) where T : IKeyboardButton;

    IEnumerable<T> Localize<T>(IEnumerable<T> buttons, string? languageCode) where T : IKeyboardButton
        => buttons.Select(x => Localize(x, languageCode));

    IEnumerable<IEnumerable<T>> Localize<T>(IEnumerable<IEnumerable<T>> buttonss, string? languageCode) where T : IKeyboardButton
        => buttonss.Select(x => Localize(x, languageCode));
}