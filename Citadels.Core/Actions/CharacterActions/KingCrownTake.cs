namespace Citadels.Core.Actions.CharacterActions;

internal class KingCrownTake : AutomaticAction
{
    public override void Execute(Game game)
    {
        game.SetCrownOwner(game.CurrentTurn.Player);
    }
}
