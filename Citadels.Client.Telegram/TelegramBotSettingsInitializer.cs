using Citadels.Client.Telegram.Resources;
using System.Globalization;
using System.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Citadels.Client.Telegram;

public class TelegramBotSettingsInitializer
{
    private readonly ITelegramBotClient _telegram;
    private readonly IStringsProvider _stringProvider;

    public TelegramBotSettingsInitializer(ITelegramBotClient telegram, IStringsProvider stringProvider)
    {
        _telegram = telegram;
        _stringProvider = stringProvider;
    }

    public async Task SetCommands()
    {
        string[] languageCodes = { "ru", "en", "pt" };
        var defaultLanguge = "ru";

        var commandStrings = new[]
        {
            (Command: "start", Str: "StartNewGame")
        };

        foreach (var languageCode in languageCodes)
        {
            await _telegram.SetMyCommandsAsync(
                commandStrings.Select(x => new BotCommand 
                { 
                    Command = x.Command, 
                    Description = _stringProvider.Get(x.Str, languageCode)! 
                }),
                BotCommandScope.AllPrivateChats(),
                languageCode == defaultLanguge ? null : languageCode);
        }
    }
}
