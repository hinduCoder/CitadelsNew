using Citadels.Client.Telegram.Resources;
using Citadels.Client.Telegram.TelegramExnteions;
using HandlebarsDotNet;

namespace Citadels.Client.Telegram.Templates;
public class HandlerbarsInitializer
{
    private readonly IStringsProvider _stringProvider;

    public HandlerbarsInitializer(IStringsProvider stringProvider)
    {
        _stringProvider = stringProvider;
    }

    public void Initialize()
    {
        Handlebars.RegisterHelper("res", ResourcesHelper);
        Handlebars.RegisterHelper("emoji", (writer, _, args) =>
        {
            if (args.Length != 1 || args[0] is not int number)
            {
                return;
            }
            writer.WriteSafeString(number switch
            {
                0 => "0️⃣",
                1 => "1️⃣",
                2 => "2️⃣",
                3 => "3️⃣",
                4 => "4️⃣",
                5 => "5️⃣",
                6 => "6️⃣",
                7 => "7️⃣",
                8 => "8️⃣",
                9 => "9️⃣",
                10 => "🔟",
                _ => number
            });
        });
        Handlebars.RegisterHelper("ulink", (writer, _, args) =>
        {
            writer.WriteSafeString(UserLink.CreateHtml(args.At<long>(0), args.At<string>(1)));
        });
    }

    void ResourcesHelper(in EncodedTextWriter writer, in HelperOptions options, in Context context, in Arguments args)
    {
        writer.WriteSafeString(_stringProvider.Get(string.Join(string.Empty, args), options.Data.Value<string>("Lang")));
    }
}
