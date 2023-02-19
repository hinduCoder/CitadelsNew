using Citadels.Core.Characters;
using Citadels.Core.Districts;

namespace Citadels.Core.Actions.CharacterActions;

internal class GatherCoinsFromBuiltDistrictsAction : ISimpleAction
{
    public void Execute(Game game)
    {
        var player = game.CurrentTurn.Player;
        var distinctKind = player.CurrentCharacter switch
        {
            King => DistrictKind.Noble,
            Bishop => DistrictKind.Religious,
            Merchant => DistrictKind.Trade,
            Warlord => DistrictKind.Military,
            _ => throw new InvalidOperationException()
        };

        player.Coins += player.BuiltDistricts.Count(x => x.Kind == distinctKind);
    }
}
