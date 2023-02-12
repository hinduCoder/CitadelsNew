using Citadels.Core.Characters;
using Citadels.Utils;

namespace Citadels.Core.Events;

public class StartGame : IGameEvent
{
    private readonly List<string> _playersNames = new ();

    public List<Character> RandomizedCharacters { get; private set; }

    public StartGame()
    {
        RandomizedCharacters = new(Character.Pool);
        RandomizedCharacters.Shuffle();
    }
    public StartGame(IEnumerable<string> playersNames)
        :this()
    {
        _playersNames.AddRange(playersNames);
    }

    public void Handle(Game game)
    {
        game.StartGame(_playersNames.Select(name => new Player(name)), RandomizedCharacters);
    }

    public bool IsValid(Game game) => game.Status == GameStatus.NotStarted;
}
