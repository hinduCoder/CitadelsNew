using Citadels.Core.Characters;

namespace Citadels.Core.Actions.CharacterAction;

internal class StealAction : ICharacterAction
{
    public void Execute(Game game, Character character)
    {
        var currentPlayer = game.CurrentTurn.Player;
        game.CurrentRound.CharacterRevealEvent += (victimPlayer, robberedCharacter) =>
        {
            if (!robberedCharacter.Equals(character))
            {
                return;
            }
            currentPlayer.Coins += victimPlayer.Coins;
            victimPlayer.Coins = 0;
        };
    }
}
