namespace Citadels.Core.Events;

public class GatherCoinsFromBank : IGameEvent
{
    public void Handle(Game game)
    {
        game.CurrentRound.CurrentTurn.GatherCoins();
    }

    public bool IsValid(Game game) 
        => game.Status == GameStatus.Round 
        && game.CurrentRound.CurrentTurn.GatherActionAvailable;
}
