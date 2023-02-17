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

    public bool IsValid(Game game) => 
        game.Status == GameStatus.Round 
        && game.CurrentRound.CurrentTurn.CanBuild
        && game.CurrentRound.CurrentTurn.Player.Districts.Contains(DistrictToBuild);
}
