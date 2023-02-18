using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Actions;

namespace Citadels.Core.Characters;

public class Bishop : Character
{
    public override int Rank => CharacterRanks.Bishop;
    public override bool CanDistrictsBeDesctroyed => false;
    protected Bishop()
    {
        RegisterPossibleActions(PossibleAction<GatherCoinsFromBuiltDistrictsAction>.Create(obligatory: true));
    }
}
