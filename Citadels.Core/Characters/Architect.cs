using Citadels.Core.Actions;

namespace Citadels.Core.Characters;

public class Architect : Character
{
    public override int Rank => 7;
    public override int DistrictMaxBuildCount => 3;
    internal override IReadOnlyCollection<ISimpleAction> AutomaticActions { get; } = new[] { ActionPool.ArchitechFreeDistricts };

}
