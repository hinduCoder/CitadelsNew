using Citadels.Core.Districts;

namespace Citadels.Core.Actions.CharacterActions;

internal class ExchangeCardsWithDeckAction : ISimpleAction<IReadOnlyCollection<District>>
{
    public void Execute(Game game, IReadOnlyCollection<District> exchangingDistricts)
    {
        var player = game.CurrentTurn.Player;
        var districtsCount = exchangingDistricts.Count;
        foreach (var districts in exchangingDistricts)
        {
            player.FoldDistrict(districts);
        }
        game.DistrictDeck.PutUnder(exchangingDistricts);

        player.AddDistricts(game.DistrictDeck.Take(districtsCount));
    }
}
