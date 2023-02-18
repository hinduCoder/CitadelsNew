using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Characters;

public class Assasin : Character
{
    public override int Rank => 1;

    internal override IReadOnlyCollection<IPossibleAction> AvailableActions => new[] { new PossibleAction<KillAction>(ActionPool.Kill) };
}
