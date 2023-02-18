using Citadels.Core.Actions;

namespace Citadels.Core.Characters;

public class Merchant : Character
{
    public override int Rank => 6;
    internal override IReadOnlyCollection<ISimpleAction> AutomaticActions { get; } = new[] { ActionPool.MerchantFreeCoin };
}
