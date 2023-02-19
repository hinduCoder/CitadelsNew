using Citadels.Core.Actions;
using Citadels.Core.Actions.DistrictActions;

namespace Citadels.Core.Districts.Special;

[DistrictName("Smithy")]
internal class Smithy : District
{
    public Smithy(string name, DistrictKind kind, int buildPrice)
        : base(name, kind, buildPrice)
    {
        RegisterPossibleActions(PossibleAction<TakeDistrictsForCoinsAction>.Create());
    }
}