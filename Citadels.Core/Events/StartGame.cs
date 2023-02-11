namespace Citadels.Core.Events;

public class StartGame : IGameEvent
{
    public void Handle(Game game)
    {
        throw new NotImplementedException();
    }

    public bool IsValid(Game game) => game.Status == GameStatus.NotStarted;

    public void Undo(Game game) => throw new NotImplementedException();
}
