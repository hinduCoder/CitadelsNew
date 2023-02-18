namespace Citadels.Core.Actions;

internal class KingCrownTake : AutomaticAction
{
    public override void Execute(Game game)
    {
        game.SetCrownOwner(game.CurrentTurn.Player);
    }
}
