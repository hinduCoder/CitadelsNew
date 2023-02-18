using Citadels.Core.Actions.CharacterAction;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Actions;

internal static class ActionPool
{
    internal static KillAction Kill = new ();
    internal static StealAction Steal= new ();
    internal static KingCrownTake TakeCrown = new();
    internal static MerchantFreeCoin MerchantFreeCoin = new();
    internal static ArchitechFreeDistricts ArchitechFreeDistricts = new();
}
