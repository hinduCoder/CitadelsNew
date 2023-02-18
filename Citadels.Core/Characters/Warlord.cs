using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Actions;

namespace Citadels.Core.Characters;

public class Warlord : Character
{
    public override int Rank => CharacterRanks.Warlord;

    protected Warlord()
    {
        RegisterPossibleActions(
            PossibleAction<GatherCoinsFromBuiltDistrictsAction>.Create(obligatory: true),
            PossibleAction<DestroyDistrictAction>.Create());
    }
}