using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Characters;

namespace Citadels.Core.Events;

public class GatherCoinsFromBuiltDistricts : IGameEvent
{
    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(new GatherCoinsFromBuiltDistrictsAction());
    }

    public bool IsValid(Game game) 
        => game is
        {
            Status: GameStatus.Round,
            CurrentTurn.Player.CurrentCharacter.Rank:
                CharacterRanks.King or CharacterRanks.Bishop or CharacterRanks.Merchant or CharacterRanks.Warlord
        } && game.CurrentTurn.ActionAvaialble<GatherCoinsFromBuiltDistrictsAction>();
}
