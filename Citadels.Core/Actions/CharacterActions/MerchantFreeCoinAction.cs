namespace Citadels.Core.Actions.CharacterActions;

internal class MerchantFreeCoinAction : AutomaticAction
{
    public override void Execute(Game game)
    {
        game.CurrentTurn.Player.Coins += 1;
    }
}
