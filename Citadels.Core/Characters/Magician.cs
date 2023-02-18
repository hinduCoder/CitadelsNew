using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class Magician : Character
{
    public override int Rank => CharacterRanks.Magician;
    protected Magician()
    {
        RegisterPossibleActions(PossibleActionCombined<ExchangeCardsWithDeckAction, ExchangeCardsWithPlayerAction>.Create());
    }
}
