using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Events;

public class ExchangeCardsWithDeck : IGameEvent
{
    public int Count { get; private set; }

    public ExchangeCardsWithDeck(int count)
    {
        Count = count;
    }
    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(new ExchangeCardsWithDeckAction(), Count);
    }

    public bool IsValid(Game game)
        => game is { Status: GameStatus.Round }
        && game.CurrentTurn.ActionAvaialble<ExchangeCardsWithDeckAction>();
}
