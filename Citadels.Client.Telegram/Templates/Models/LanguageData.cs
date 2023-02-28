namespace Citadels.Client.Telegram.Templates.Models;
public class LanguageData
{
    public string? Lang { get; }
	public LanguageData(string? lang) => Lang = lang;

    public static implicit operator LanguageData(string? lang) => new (lang);
}
