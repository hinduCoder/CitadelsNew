using Citadels.Core.Characters;
using Citadels.Core.Districts;
using Citadels.Utils;

namespace Citadels.Core.Events;

public class StartGame : IGameEvent
{
    private readonly List<string> _playersNames = new ();
    private readonly List<Character> _randomizedCharacters = new();
    private readonly List<District> _randomizedDistricts = new();

    public IReadOnlyList<string> PlayerNames => _playersNames;
    public IReadOnlyList<Character> RandomizedCharacters => _randomizedCharacters;
    public IReadOnlyList<District> RandomizedDistricts => _randomizedDistricts;

    public StartGame(IEnumerable<string> playersNames)
    {
        _randomizedCharacters.AddRange(Character.Pool);
        _randomizedCharacters.Shuffle();

        _randomizedDistricts.AddRange(District.Pool);
        _randomizedDistricts.Shuffle();

        _playersNames.AddRange(playersNames);
    }

    public void Handle(Game game)
    {
        game.StartGame(_playersNames.Select(name => new Player(name)), RandomizedCharacters, RandomizedDistricts);
    }

    public bool IsValid(Game game) 
        => game.Status == GameStatus.NotStarted
        && _playersNames.Count <= 7
        && _playersNames.Count >= 4; //special rules for two or three players are not going to be supported at first stage though we can allow to play them by regular rules

    private StartGame()
    {

    }
}
