namespace Citadels.Client.Telegram.Templates.Models;

public record ChatMessage(string? Language, UserModel User, string? message) 
    : ModelBase(Language);