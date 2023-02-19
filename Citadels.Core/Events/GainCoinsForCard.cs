using Citadels.Core.Actions.DistrictActions;

namespace Citadels.Core.Events;

internal class GainCoinsForCard : IGameEvent
{
    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(new GainCoinsForCardAction());
    }

    public bool IsValid(Game game)
        => game is
        {
            Status: GameStatus.Round
        } && game.CurrentTurn.ActionAvaialble<GainCoinsForCardAction>();
}
