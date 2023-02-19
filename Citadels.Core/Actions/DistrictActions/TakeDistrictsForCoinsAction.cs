using Citadels.Core.Districts;

namespace Citadels.Core.Actions.DistrictActions;

internal class TakeDistrictsForCoinsAction : ISimpleAction<District>
{
    public void Execute(Game game, District district)
    {
        game.CurrentTurn.Player.FoldDistrict(district);
        game.DistrictDeck.PutUnder(district);
        game.CurrentTurn.Player.Coins += 2;
    }
}