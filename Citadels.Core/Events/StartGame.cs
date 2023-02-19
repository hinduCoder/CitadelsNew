using Citadels.Core.Characters;
using Citadels.Core.Districts;
using Citadels.Utils;

namespace Citadels.Core.Events;

public class StartGame : IGameEvent
{
    private readonly List<string> _playersNames = new ();

    public IReadOnlyList<string> PlayerNames => _playersNames;
    public int RandomSeed { get; private set; }

    public StartGame(IEnumerable<string> playersNames, int randomSeed)
    {
        _playersNames.AddRange(playersNames);
        RandomSeed = randomSeed;
    }

    public void Handle(Game game)
    {
        var districtPool = District.Pool.ToList();
        districtPool.Shuffle(RandomSeed);
        game.StartGame(_playersNames.Select(name => new Player(name)), districtPool);
    }

    public bool IsValid(Game game) 
        => game.Status == GameStatus.NotStarted
        && _playersNames.Count is >= 4 and <= 7; //special rules for two or three players are not going to be supported at first stage though we can allow to play them by regular rules

    private StartGame()
    {

    }
}
