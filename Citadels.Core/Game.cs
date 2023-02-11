namespace Citadels.Core;

public class Game
{
    private List<Player> _players = new ();

    public GameStatus Status { get; private set; } = GameStatus.NotStarted;
    public IReadOnlyList<Player> Players => _players;

    internal void StartGame(IEnumerable<Player> players)
    {
        _players.AddRange(players);
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