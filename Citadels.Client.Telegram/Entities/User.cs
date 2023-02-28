using Citadels.Client.Telegram.TelegramExnteions;

namespace Citadels.Client.Telegram.Entities;

public class User
{
    public long TelegramUserId { get; }
    public long PrivateChatId { get; private set; }
    public int? UpdatingTelegramMessageId { get; set; }
    public string? Name { get; set; }
    public string? LanguageCode { get; set; }
    public Guid? CurrentGameId { get; set;  }
    public Game? CurrentGame { get; set; }

    public string TelegramLinkMarkdown => UserLink.CreateMarkdown(TelegramUserId, Name ?? TelegramUserId.ToString());
    public string TelegramLinkHtml => UserLink.CreateHtml(TelegramUserId, Name ?? TelegramUserId.ToString());

    public User(long telegramUserId, long privateChatId)
    {
        TelegramUserId = telegramUserId;
        PrivateChatId = privateChatId; 
    }

    private User() 
    { 
    }
}
