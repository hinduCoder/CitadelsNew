using Citadels.Core.Characters;

namespace Citadels.Core.Actions.CharacterActions;

internal class KillAction : ICharacterAction
{
    public bool Obligatory => false;
    public void Execute(Game game, Character character)
    {
        var victimPlayer = game.Players.SingleOrDefault(x => x.CurrentCharacter.Equals(character));
        if (victimPlayer is null)
        {
            return;
        }
        victimPlayer.IsAlive = false;
    }
}
