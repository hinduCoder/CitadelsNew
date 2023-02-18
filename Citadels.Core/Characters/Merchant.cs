using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class Merchant : Character
{
    public override int Rank => CharacterRanks.Merchant;
    protected Merchant()
    {
        RegisterAutomaticActions(new MerchantFreeCoinAction());
        RegisterPossibleActions(PossibleAction<GatherCoinsFromBuiltDistrictsAction>.Create(obligatory: true));
    }
}
