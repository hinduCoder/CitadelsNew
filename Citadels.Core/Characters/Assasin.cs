using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class Assasin : Character
{
    public override int Rank => CharacterRanks.Assasin;

    protected Assasin()
    {
        RegisterPossibleActions(PossibleAction<KillAction>.Create());
    }
}
