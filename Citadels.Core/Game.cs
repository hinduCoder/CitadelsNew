namespace Citadels.Core;

public class Game
{
    public GameStatus Status { get; private set; } = GameStatus.NotStarted;

    internal void StartGame()
    {
        Status = GameStatus.Draft;
        //TODO launch draft;
    }
}

public enum GameStatus
{
    NotStarted,
    Draft,
    Round,
    Ended
}