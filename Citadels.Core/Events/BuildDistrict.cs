using Citadels.Core.Districts;

namespace Citadels.Core.Events;

public class BuildDistrict : IGameEvent
{
    public District DistrictToBuild { get; private set; }

    public BuildDistrict(District districtToBuild)
    {
        DistrictToBuild = districtToBuild;
    }

    public void Handle(Game game)
    {
        game.CurrentRound.CurrentTurn.BuildDistrict(DistrictToBuild);
    }

    public bool IsValid(Game game)
    {
        var currentTurn = game.CurrentRound.CurrentTurn;
        return game.Status == GameStatus.Round
        && currentTurn.CanBuild
        && currentTurn.Player.Districts.Contains(DistrictToBuild)
        && !currentTurn.Player.BuiltDistricts.Contains(DistrictToBuild)
        && currentTurn.Player.Coins >= DistrictToBuild.BuildPrice;
    }
}
