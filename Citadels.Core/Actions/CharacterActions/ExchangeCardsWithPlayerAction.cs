namespace Citadels.Core.Actions.CharacterActions;

internal class ExchangeCardsWithPlayerAction : IPlayerAction
{
    public bool Obligatory => false;

    public void Execute(Game game, Player victimPlayer)
    {
        var currentPlayer = game.CurrentTurn.Player;
        currentPlayer.ExchageDistrictsWithOtherPlayer(victimPlayer);
    }
}
