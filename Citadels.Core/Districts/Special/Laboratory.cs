using Citadels.Core.Actions;
using Citadels.Core.Actions.DistrictActions;

namespace Citadels.Core.Districts.Special;

[DistrictName("Laboratory")]
internal class Laboratory : District
{
    public override int Points => BuildPrice + 2;
    public Laboratory(string name, DistrictKind kind, int buildPrice)
        : base(name, kind, buildPrice)
    {
        RegisterPossibleActions(PossibleAction<GainCoinsForCardAction>.Create());
    }
}
