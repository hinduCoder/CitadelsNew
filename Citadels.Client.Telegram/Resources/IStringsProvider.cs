using System.Collections.Concurrent;
using System.Globalization;
using System.Resources;

namespace Citadels.Client.Telegram.Resources;

public interface IStringsProvider
{
    string? Get(string name, string? languageCode);
}

public class StringProvider : IStringsProvider
{
    private readonly ResourceManager _resourceManager;
    private readonly ConcurrentDictionary<string, CultureInfo> _cultureCache = new();

    public StringProvider(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    public string? Get(string name, string? languageCode)
    {
        if (languageCode is null) 
        {
            return _resourceManager.GetString(name);
        }
        var culture = _cultureCache.GetOrAdd(languageCode, CultureInfo.CreateSpecificCulture);
        return _resourceManager.GetString(name, culture);
    }
}