using Citadels.Client.Telegram.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Citadels.Client.Telegram;

public class TelegramBotSettingsInitializer
{
    private static readonly string[] LanguageCodes = { "ru", "en" };
    private const string DefaultLanguage = "en";

    private readonly ITelegramBotClient _telegram;
    private readonly IStringsProvider _stringProvider;

    public TelegramBotSettingsInitializer(ITelegramBotClient telegram, IStringsProvider stringProvider)
    {
        _telegram = telegram;
        _stringProvider = stringProvider;
    }

    public async Task SetCommands()
    {
        var commandStrings = new[]
        {
            (Command: "start", Str: "StartNewGame")
        };

        foreach (var languageCode in LanguageCodes)
        {
            await _telegram.SetMyCommandsAsync(
                commandStrings.Select(x => new BotCommand 
                { 
                    Command = x.Command, 
                    Description = _stringProvider.Get(x.Str, languageCode)! 
                }),
                BotCommandScope.AllPrivateChats(),
                languageCode == DefaultLanguage ? null : languageCode);
            await _telegram.SetMyCommandsAsync(
                commandStrings.Select(x => new BotCommand
                {
                    Command = x.Command,
                    Description = _stringProvider.Get(x.Str, languageCode)!
                }).Concat(new[]
                {
                    new BotCommand { Command = "test", Description = "Test" }
                }),
                BotCommandScope.Chat(628413),
                languageCode == DefaultLanguage ? null : languageCode);
        }
    }
}
