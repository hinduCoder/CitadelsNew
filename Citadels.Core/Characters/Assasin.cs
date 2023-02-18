using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class Assasin : Character
{
    public override int Rank => CharacterRanks.Assasin;

    internal override IReadOnlyCollection<IPossibleAction> AvailableActions => new[] { PossibleAction<KillAction>.Create() };
}
