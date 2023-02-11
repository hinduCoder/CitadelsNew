namespace Citadels.Core;

public class GameEventHandler
{
    public bool HandleEvent(Game game, IGameEvent gameEvent)
    {
        if (!gameEvent.IsValid(game))
        {
            return false;
        }
        gameEvent.Handle(game);
        return true;
    }
}
