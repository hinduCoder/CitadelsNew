using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace Citadels.Client.Telegram.TelegramExnteions;

public static partial class MessageExnteions
{
    [GeneratedRegex(@"^\/(?<command>[a-z_\d]+)(?:@\S+)?(?: (?<param>.*))?$")]
    private static partial Regex CommandRegex();
    public static Command? ParseCommand(this Message message)
    {
        if (message?.Text is null)
        {
            return default;
        }
        var match = CommandRegex().Match(message.Text);
        if (!match.Success) 
        {
            return default;
        }
        return new Command(match.Groups["command"].Value, match.Groups["param"].Value);
    }
}
