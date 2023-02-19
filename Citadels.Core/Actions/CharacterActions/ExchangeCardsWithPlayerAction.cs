namespace Citadels.Core.Actions.CharacterActions;

internal class ExchangeCardsWithPlayerAction : IPlayerAction
{
    public void Execute(Game game, Player victimPlayer)
    {
        var currentPlayer = game.CurrentTurn.Player;

        var otherPlayerDistricts = victimPlayer.Districts.ToList();

        victimPlayer.ClearDistricts();
        victimPlayer.AddDistricts(currentPlayer.Districts);

        currentPlayer.ClearDistricts();
        currentPlayer.AddDistricts(otherPlayerDistricts);
    }
}
