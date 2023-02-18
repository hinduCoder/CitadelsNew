using Citadels.Core.Actions;
using Citadels.Core.Actions.CharacterActions;

namespace Citadels.Core.Events;

public class ExchangeCardsWithPlayer : IGameEvent
{
    public string VictimName { get; private set; }

    public ExchangeCardsWithPlayer(string victimName)
    {
        VictimName = victimName;
    }
    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(new ExchangeCardsWithPlayerAction(), game.Players.Single(x => x.Name == VictimName));
    }

    public bool IsValid(Game game)
        => game is { Status: GameStatus.Round }
        && game.CurrentTurn.ActionAvaialble<ExchangeCardsWithPlayerAction>();
}
