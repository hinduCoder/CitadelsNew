using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Characters;

namespace Citadels.Core.Events;

public class Kill : IGameEvent
{
    public Character Victim { get; private set; }

    public Kill(Character victim)
    {
        Victim = victim;
    }

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
