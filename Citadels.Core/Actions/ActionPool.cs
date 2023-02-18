using Citadels.Core.Actions.CharacterAction;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Actions;

internal static class ActionPool
{
    internal static KillAction Kill = new ();
    internal static StealAction Steal = new ();
    internal static ExchangeCardsWithDeckAction ExchangeCardsWithDeck = new();
    internal static ExchangeCardsWithPlayerAction ExchangeCardsWithPlayer = new();
    internal static KingCrownTakeAction TakeCrown = new();
    internal static MerchantFreeCoinAction MerchantFreeCoin = new();
    internal static ArchitechFreeDistrictsAction ArchitechFreeDistricts = new();
}
