using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class Merchant : Character
{
    public override int Rank => CharacterRanks.Merchant;
    internal override IReadOnlyCollection<ISimpleAction> AutomaticActions { get; } = new[] { ActionPool.MerchantFreeCoin };
    internal override IReadOnlyCollection<IPossibleAction> AvailableActions { get; }
        = new[] { new PossibleAction<GatherCoinsFromBuiltDistrictsAction>(ActionPool.GatherCoinsFromBuiltDistricts) };
}
