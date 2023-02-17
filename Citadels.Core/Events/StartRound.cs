namespace Citadels.Core.Events;

public class StartRound : IGameEvent
{
    public void Handle(Game game)
    {
        game.StartRound();
    }

    public bool IsValid(Game game) => game.Status == GameStatus.Draft && game.Draft.Completed;
}
