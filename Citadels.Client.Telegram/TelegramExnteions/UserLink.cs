namespace Citadels.Client.Telegram.TelegramExnteions;
public static class UserLink
{
    public static string CreateMarkdown(long userId, string userName)
        => $"[{userName}](tg://user?id={userId})";
    public static string CreateHtml(long userId, string userName)
        => $@"<a href=""tg://user?id={userId}"">{userName}</a>";
}
