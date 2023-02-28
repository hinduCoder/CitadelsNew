namespace Citadels.Client.Telegram.Templates.Models;

public record ChatMessage(UserModel User, string? message);