namespace Citadels.Client.Telegram.Templates.Models;

public record RegistrationContext(string? Language, IEnumerable<UserModel> Users)
    : ModelBase(Language);
