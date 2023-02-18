using Citadels.Core.Actions;
using Citadels.Core.Characters;

namespace Citadels.Core.Events;

public class Kill : IGameEvent
{
    public Kill(Character victim)
    {
        Victim = victim;
    }

    public Character Victim { get; private set; }

    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(ActionPool.Kill, Victim);
    }

    public bool IsValid(Game game)
        => game is { Status: GameStatus.Round } 
        && game.CurrentTurn.Player.CurrentCharacter.Is<Assasin>()
        && game.CurrentTurn.ActionAvaialble<KillAction>()
        && !Victim.Is<Assasin>();
}
