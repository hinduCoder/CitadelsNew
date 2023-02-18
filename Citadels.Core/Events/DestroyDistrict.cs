using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Characters;
using Citadels.Core.Districts;

namespace Citadels.Core.Events;

public class DestroyDistrict : IGameEvent
{
    public Player Victim { get; private set; }
    public District DistrictToDestroy { get; private set; }

    public DestroyDistrict(Player victim, District districtToDestroy)
    {
        Victim = victim;
        DistrictToDestroy = districtToDestroy;
    }

    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(new DestroyDistrictAction(), Victim, DistrictToDestroy);
    }

    public bool IsValid(Game game)
        => game is { Status: GameStatus.Round, CurrentTurn.Player.CurrentCharacter.Rank: CharacterRanks.Warlord }
        && game.CurrentTurn.ActionAvaialble<DestroyDistrictAction>();
}
