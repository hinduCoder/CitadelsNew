namespace Citadels.Core.Events;

public class GatherDistrict : IGameEvent
{
    public void Handle(Game game)
    {
        game.CurrentRound.CurrentTurn.GatherDistrict();
    }

    public bool IsValid(Game game) 
        => game.Status == GameStatus.Round 
        && game.CurrentRound.CurrentTurn.GatherActionAvailable;
}
