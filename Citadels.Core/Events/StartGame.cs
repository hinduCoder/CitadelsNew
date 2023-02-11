namespace Citadels.Core.Events;

public class StartGame : IGameEvent
{
    private readonly List<string> _playersNames = new ();
    public StartGame(IEnumerable<string> playersNames)
    {
        _playersNames.AddRange(playersNames);
    }

    public void Handle(Game game)
    {
        game.StartGame(_playersNames.Select(name => new Player(name)));
    }

    public bool IsValid(Game game) => game.Status == GameStatus.NotStarted;

    public void Undo(Game game) => throw new NotImplementedException();
}
