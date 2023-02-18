using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterAction;
using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Characters;

namespace Citadels.Core.Events;

public class Steal : IGameEvent
{
    public Character Victim { get; private set; }

    public Steal(Character victim)
    {
        Victim = victim;
    }

    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(ActionPool.Steal, Victim);
    }

    public bool IsValid(Game game)
        => game is { Status: GameStatus.Round }
        && game.CurrentTurn.Player.CurrentCharacter.Is<Thief>()
        && game.CurrentTurn.ActionAvaialble<StealAction>()
        && !Victim.Is<Assasin>() && !Victim.Is<Thief>()
        && game.Players.SingleOrDefault(x => x.CurrentCharacter.Equals(Victim))?.IsAlive == true;
}
