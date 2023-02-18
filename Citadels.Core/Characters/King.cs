using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class King : Character
{
    public override int Rank => CharacterRanks.King;
    protected King()
    {
        RegisterPossibleActions(PossibleAction<GatherCoinsFromBuiltDistrictsAction>.Create(obligatory: true));
        RegisterAutomaticActions(new KingCrownTakeAction());
    }
}
