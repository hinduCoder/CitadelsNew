namespace Citadels.Core.Actions.CharacterActions;

internal class ExchangeCardsWithPlayerAction : IPlayerAction
{
    public void Execute(Game game, Player victimPlayer)
    {
        var currentPlayer = game.CurrentTurn.Player;
        currentPlayer.ExchageDistrictsWithOtherPlayer(victimPlayer);
    }
}
