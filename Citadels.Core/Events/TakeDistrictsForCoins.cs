using Citadels.Core.Actions.DistrictActions;
using Citadels.Core.Districts;

namespace Citadels.Core.Events;

internal class TakeDistrictsForCoins : IGameEvent
{
    public District DistrictToChange { get; private set; }

    public TakeDistrictsForCoins(District districtToChange)
    {
        DistrictToChange = districtToChange;
    }

    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(new TakeDistrictsForCoinsAction(), DistrictToChange);
    }

    public bool IsValid(Game game)
        => game is
        {
            Status: GameStatus.Round
        } && game.CurrentTurn.Player.Districts.Contains(DistrictToChange)
        && game.CurrentTurn.ActionAvaialble<TakeDistrictsForCoinsAction>();
}