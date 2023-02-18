using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Actions;

namespace Citadels.Core.Characters;

public class Warlord : Character
{
    public override int Rank => CharacterRanks.Warlord;
    internal override IReadOnlyCollection<IPossibleAction> AvailableActions { get; }
        = new[] { new PossibleAction<GatherCoinsFromBuiltDistrictsAction>(ActionPool.GatherCoinsFromBuiltDistricts) };
}