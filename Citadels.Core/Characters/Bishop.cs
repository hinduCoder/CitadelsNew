using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Actions;

namespace Citadels.Core.Characters;

public class Bishop : Character
{
    public override int Rank => CharacterRanks.Bishop;
    public override bool CanDistrictsBeDesctroyed => false;
    internal override IReadOnlyCollection<IPossibleAction> AvailableActions { get; }
        = new[] { PossibleAction<GatherCoinsFromBuiltDistrictsAction>.Create() };
}
