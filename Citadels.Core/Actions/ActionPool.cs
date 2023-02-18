using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Actions;

internal static class ActionPool
{
    internal static KillAction Kill = new ();
    internal static ISimpleAction TakeCrown = new KingCrownTake();
    internal static ISimpleAction MerchantFreeCoin = new MerchantFreeCoin();
    internal static ISimpleAction ArchitechFreeDistricts = new ArchitechFreeDistricts();
}
