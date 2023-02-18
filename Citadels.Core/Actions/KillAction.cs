using Citadels.Core.Characters;

namespace Citadels.Core.Actions;

internal class KillAction : ICharacterAction
{
    public bool Obligatory => false;
    public void Execute(Game game, Character character)
    {
        game.Players.Single(x => x.CurrentCharacter.Equals(character)).IsAlive = false;
    }
}
