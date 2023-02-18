using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterAction;

namespace Citadels.Core.Characters;

public class Thief : Character
{
    public override int Rank => CharacterRanks.Thief;
    internal override IReadOnlyCollection<IPossibleAction> AvailableActions { get; } = new [] { PossibleAction<StealAction>.Create() };
}
