using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Characters;
using Citadels.Core.Districts;
using Citadels.Core.Districts.Special;

namespace Citadels.Core.Events;

public class DestroyDistrict : IGameEvent
{
    public string VictimPlayerName { get; private set; }
    public Player? Victim { get; private set; }
    public District DistrictToDestroy { get; private set; }

    public DestroyDistrict(string victimPlayerName, District districtToDestroy)
    {
        VictimPlayerName = victimPlayerName;
        DistrictToDestroy = districtToDestroy;
    }

    public DestroyDistrict(Player victim, District districtToDestroy)
        :this (victim.Name, districtToDestroy)
    {
        Victim = victim;
    }

    public void Handle(Game game)
    {
        game.CurrentTurn.DestroyDistrict(GetVictim(game), DistrictToDestroy);
    }

    public bool IsValid(Game game)
        => DistrictToDestroy.CanBeDestroyed
        && game is { Status: GameStatus.Round, CurrentTurn.Player.CurrentCharacter.Rank: CharacterRanks.Warlord }
    && game.CurrentTurn.ActionAvaialble<DestroyDistrictAction>()
        && game.CurrentTurn.Player.Coins >= DistrictToDestroy.BuildPrice
            - (GetVictim(game).HasDistrictOfType<GreatWall>() && DistrictToDestroy is not GreatWall ? 0 : 1);

    private Player GetVictim(Game game) => Victim ?? game.Players.Single(x => x.Name == VictimPlayerName);
}
