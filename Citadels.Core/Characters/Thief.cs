using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterAction;

namespace Citadels.Core.Characters;

public class Thief : Character
{
    public override int Rank => 2;
    internal override IReadOnlyCollection<IPossibleAction> AvailableActions { get; } = new [] { new PossibleAction<StealAction>(ActionPool.Steal) };
}
