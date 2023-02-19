namespace Citadels.Core.Actions.DistrictActions;

internal class RestoreDestroyedDistrictAction : IPlayerAction
{
    public void Execute(Game game, Player targetPlayer)
    {
        var district = game.CurrentTurn.LastDestroyedDistrict;
        if (district is null)
        {
            return;
        }
        targetPlayer.BuildDistrict(district);
        targetPlayer.Coins--;
    }
}