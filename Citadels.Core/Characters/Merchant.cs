using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class Merchant : Character
{
    public override int Rank => CharacterRanks.Merchant;
    internal override IReadOnlyCollection<ISimpleAction> AutomaticActions { get; } = new[] { new MerchantFreeCoinAction() };
    internal override IReadOnlyCollection<IPossibleAction> AvailableActions { get; }
        = new[] { PossibleAction<GatherCoinsFromBuiltDistrictsAction>.Create() };
}
