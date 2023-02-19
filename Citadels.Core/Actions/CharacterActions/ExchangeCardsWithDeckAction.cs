namespace Citadels.Core.Actions.CharacterActions;

internal class ExchangeCardsWithDeckAction : ISimpleAction<int>
{
    public void Execute(Game game, int exchangeCount)
    {
        var player = game.CurrentTurn.Player;
        var districtsCount = player.Districts.Count;
        game.DistrictDeck.PutUnder(player.Districts);
        player.ClearDistricts();
        player.AddDistricts(game.DistrictDeck.Take(districtsCount)); //TODO count can be arbitrary
    }
}
