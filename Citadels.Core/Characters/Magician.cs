using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class Magician : Character
{
    public override int Rank => CharacterRanks.Magician;
    internal override IReadOnlyCollection<IPossibleAction> AvailableActions { get; } 
        = new[] { new PossibleActionCombined<ExchangeCardsWithDeckAction, ExchangeCardsWithPlayerAction>(
            ActionPool.ExchangeCardsWithDeck, ActionPool.ExchangeCardsWithPlayer) };
}
