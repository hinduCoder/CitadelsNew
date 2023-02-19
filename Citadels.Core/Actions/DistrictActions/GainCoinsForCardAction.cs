namespace Citadels.Core.Actions.DistrictActions;

internal class GainCoinsForCardAction : ISimpleAction
{
    public void Execute(Game game)
    {
        var player = game.CurrentTurn.Player;
        player.Coins -= 2;
        player.AddDistricts(game.DistrictDeck.Take(3));
    }
}
