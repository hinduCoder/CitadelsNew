using Citadels.Core.Actions.CharacterActions;
using Citadels.Core.Districts;

namespace Citadels.Core.Events;

public class ExchangeCardsWithDeck : IGameEvent
{
    public List<District> DistrictsToExchange { get; private set; }

    public ExchangeCardsWithDeck(List<District> districtsToExchange)
    {
        DistrictsToExchange = districtsToExchange;
    }

    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(new ExchangeCardsWithDeckAction(), DistrictsToExchange);
    }

    public bool IsValid(Game game)
        => game is { Status: GameStatus.Round }
        && game.CurrentTurn.ActionAvaialble<ExchangeCardsWithDeckAction>()
        && game.CurrentTurn.Player.Districts.Intersect(DistrictsToExchange).Count() == DistrictsToExchange.Count;
}
