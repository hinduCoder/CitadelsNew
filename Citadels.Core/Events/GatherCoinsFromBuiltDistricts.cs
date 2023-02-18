using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Characters;

namespace Citadels.Core.Events;

internal class GatherCoinsFromBuiltDistricts : IGameEvent
{
    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(ActionPool.GatherCoinsFromBuiltDistricts);
    }

    public bool IsValid(Game game) 
        => game is
        {
            Status: GameStatus.Round,
            CurrentTurn.Player.CurrentCharacter.Rank:
                CharacterRanks.King or CharacterRanks.Bishop or CharacterRanks.Merchant or CharacterRanks.Warlord
        } && game.CurrentTurn.ActionAvaialble<GatherCoinsFromBuiltDistrictsAction>();
}
