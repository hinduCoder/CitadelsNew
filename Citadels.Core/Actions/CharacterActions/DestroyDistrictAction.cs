using Citadels.Core.Districts;
using Citadels.Core.Districts.Special;

namespace Citadels.Core.Actions.CharacterActions;

internal class DestroyDistrictAction : IPlayerDistrictAction
{
    public void Execute(Game game, Player player, District district)
    {
        if (!district.CanBeDestroyed)
        {
            return;
        }
        var destroyPrice = district.BuildPrice - 1;
        var currentPlayer = game.CurrentTurn.Player;

        currentPlayer.Coins -= destroyPrice;
        if (player.HasDistrictOfType<GreatWall>() && district is not GreatWall)
        {
            currentPlayer.Coins--;
        }
        player.DestroyDistrict(district);
        game.DistrictDeck.PutUnder(district);
    }
}
