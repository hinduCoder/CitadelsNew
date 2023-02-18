namespace Citadels.Core.Actions.CharacterActions;

internal class ArchitechFreeDistricts : AutomaticAction
{
    public override void Execute(Game game)
    {
        game.CurrentTurn.Player.AddDistricts(game.DistrictDeck.Take(2));
    }
}
