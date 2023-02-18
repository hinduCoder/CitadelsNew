namespace Citadels.Core.Actions.CharacterActions;

internal class MerchantFreeCoin : AutomaticAction
{
    public override void Execute(Game game)
    {
        game.CurrentTurn.Player.Coins += 1;
    }
}
