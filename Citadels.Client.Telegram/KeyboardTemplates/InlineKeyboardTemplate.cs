using Citadels.Client.Telegram.TelegramExnteions;
using Telegram.Bot.Types.ReplyMarkups;

namespace Citadels.Client.Telegram.KeyboardTemplates;
public class InlineKeyboardTemplate<TState>
{
    private readonly List<List<IInlineKeyboardButtonTemplate<TState>>> _keyboard = new();

    public InlineKeyboardTemplate<TState> AddRow(Action<ButtonFactory<TState>> factoryCallback)
    {
        var factory = new ButtonFactory<TState>();
        factoryCallback(factory);
        _keyboard.Add(factory.Buttons);
        return this;
    }
    public InlineKeyboardMarkup GetKeyboard(TState state) => GetKeyboard(state, null, null);
    public InlineKeyboardMarkup GetKeyboard(TState state, string? languageCode, IKeyboardLocalizator? keyboardLocalizator)
    {
        var buttons = GetButtons(state);
        if (keyboardLocalizator is not null)
        {
            buttons = keyboardLocalizator.Localize(buttons, languageCode);
        }

        return new InlineKeyboardMarkup(buttons);
    }

    private IEnumerable<IEnumerable<InlineKeyboardButton>> GetButtons(TState state)
    {
        return _keyboard
            .Select(row => row
                .Where(x => x.Show(state))
                .Select(x => x.Button))
            .Where(row => row.Any());
    }
}

