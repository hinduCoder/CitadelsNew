namespace Citadels.Client.Telegram.Resources;

public interface IStringsProvider
{
    string? Get(string name, string? languageCode);
    string? Get(string name, string? languageCode, params object[] args);
}
