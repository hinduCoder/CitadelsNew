namespace Citadels.Core.Actions.CharacterActions;

internal class ArchitechFreeDistrictsAction : AutomaticAction
{
    public override void Execute(Game game)
    {
        game.CurrentTurn.Player.AddDistricts(game.DistrictDeck.Take(2));
    }
}
