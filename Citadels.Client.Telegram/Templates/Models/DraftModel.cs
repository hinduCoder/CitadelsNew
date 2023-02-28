namespace Citadels.Client.Telegram.Templates.Models;
public record DraftModel(IEnumerable<CharacterCardModel> Available, IEnumerable<CharacterCardModel> Unavailable);
