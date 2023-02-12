namespace Citadels.Core.Events;

public class DiscardDraftFirstCards : IGameEvent
{
    public void Handle(Game game)
    {
        game.Draft.DiscardFirstCards();
    }

    public bool IsValid(Game game) => game.Status == GameStatus.Draft && !game.Draft.Started;
}
