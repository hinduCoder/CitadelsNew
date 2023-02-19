using Citadels.Core.Districts;

namespace Citadels.Core.Actions.CharacterActions;

internal class DestroyDistrictAction : IPlayerDistrictAction
{
    public void Execute(Game game, Player player, District district)
    {
        if (!district.CanBeDestroyed)
        {
            return;
        }
        var destoyPrice = district.BuildPrice - 1;
        var currentPlayer = game.CurrentTurn.Player;
        currentPlayer.Coins -= destoyPrice;
        player.DestroyDistrict(district);
        game.DistrictDeck.PutUnder(district);
    }
}
