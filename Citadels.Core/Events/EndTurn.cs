namespace Citadels.Core.Events;

public class EndTurn : IGameEvent
{
    public void Handle(Game game)
    {
        game.CurrentRound.NewTurn();
    }

    public bool IsValid(Game game) => game.Status == GameStatus.Round && game.CurrentRound.CurrentTurn.GatherActionDone;
}
