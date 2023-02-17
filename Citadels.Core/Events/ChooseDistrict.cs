using Citadels.Core.Districts;

namespace Citadels.Core.Events;

public class ChooseDistrict : IGameEvent
{
    public District DistrictChoosen { get; private set; }

    public ChooseDistrict(District districtChoosen)
    {
        DistrictChoosen = districtChoosen;
    }

    public void Handle(Game game)
    {
        game.CurrentRound.CurrentTurn.ChooseDistrict(DistrictChoosen);
    }

    public bool IsValid(Game game) => game.Status == GameStatus.Round && game.CurrentRound.CurrentTurn.GatherActionInProgress;
}
