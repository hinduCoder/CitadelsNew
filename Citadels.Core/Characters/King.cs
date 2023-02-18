using Citadels.Core.Actions;

namespace Citadels.Core.Characters;

public class King : Character
{
    public override int Rank => 4;

    internal override IReadOnlyCollection<ISimpleAction> AutomaticActions { get; } = new[] { ActionPool.TakeCrown };
}
