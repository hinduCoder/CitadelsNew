using Citadels.Core.Actions;
using Citadels.Core.Actions.DistrictActions;

namespace Citadels.Core.Districts.Special;

[DistrictName("Graveyard")]
internal class Graveyard : District
{
    public Graveyard(string name, DistrictKind kind, int buildPrice)
        : base(name, kind, buildPrice)
    {
        RegisterPossibleActions(PossibleAction<RestoreDestroyedDistrictAction>.Create());
    }
}
