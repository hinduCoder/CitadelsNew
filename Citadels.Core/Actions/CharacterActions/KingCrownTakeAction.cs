namespace Citadels.Core.Actions.CharacterActions;

internal class KingCrownTakeAction : AutomaticAction
{
    public override void Execute(Game game)
    {
        game.SetCrownOwner(game.CurrentTurn.Player);
    }
}
