using Citadels.Core.Characters;

namespace Citadels.Core;

public class Game
{
    private readonly List<Player> _players = new ();
    private Draft? _draft;

    public GameStatus Status { get; private set; } = GameStatus.NotStarted;
    public IReadOnlyList<Player> Players => _players;
    public Draft Draft => _draft ?? throw new InvalidOperationException();

    internal void StartGame(IEnumerable<Player> players, IEnumerable<Character> randomizedCharacters)
    {
        _players.AddRange(players);
        _draft = new Draft(randomizedCharacters, _players, 0); //TODO crown owner index mustn't be always 0
        Status = GameStatus.Draft;
    }
}

public enum GameStatus
{
    NotStarted,
    Draft,
    Round,
    Ended
}