namespace Citadels.Core.Events;

public class EndTurn : IGameEvent
{
    public void Handle(Game game)
    {
        game.CurrentRound.NewTurn();
    }

    public bool IsValid(Game game) => game is { Status: GameStatus.Round, CurrentTurn.CanEnd: true };
}
