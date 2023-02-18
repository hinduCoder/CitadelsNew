using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class Architect : Character
{
    public override int Rank => CharacterRanks.Architect;
    public override int DistrictMaxBuildCount => 3;
    internal override IReadOnlyCollection<ISimpleAction> AutomaticActions { get; } = new[] { new ArchitechFreeDistrictsAction() };

}
