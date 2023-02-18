using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class King : Character
{
    public override int Rank => CharacterRanks.King;

    internal override IReadOnlyCollection<ISimpleAction> AutomaticActions { get; } = new[] { ActionPool.TakeCrown };
    internal override IReadOnlyCollection<IPossibleAction> AvailableActions { get; } 
        = new[] { new PossibleAction<GatherCoinsFromBuiltDistrictsAction>(ActionPool.GatherCoinsFromBuiltDistricts) };
}
