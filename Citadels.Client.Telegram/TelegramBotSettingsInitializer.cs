using System.Globalization;
using System.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Citadels.Client.Telegram;

public class TelegramBotSettingsInitializer
{
    private readonly ITelegramBotClient _telegram;
    private readonly ResourceManager _resourceManager;

    public TelegramBotSettingsInitializer(ITelegramBotClient telegram, ResourceManager resourceManager)
    {
        _telegram = telegram;
        _resourceManager = resourceManager;
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
            var culture = CultureInfo.CreateSpecificCulture(languageCode);
            await _telegram.SetMyCommandsAsync(
                commandStrings.Select(x => new BotCommand 
                { 
                    Command = x.Command, 
                    Description = _resourceManager.GetString(x.Str, culture)! 
                }),
                BotCommandScope.AllPrivateChats(),
                languageCode == defaultLanguge ? null : languageCode);
        }
    }
}
