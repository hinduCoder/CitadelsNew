using Citadels.Core.Districts;

namespace Citadels.Core.Actions.CharacterActions;

internal class DestroyDistrictAction : IPlayerDistrictAction
{
    public void Execute(Game game, Player player, District district)
    {
        var destoyPrice = district.BuildPrice - 1;
        var currentPlayer = game.CurrentTurn.Player;
        currentPlayer.Coins -= destoyPrice;
        player.DestroyDistrict(district);
        game.DistrictDeck.PutUnder(district);
    }
}
