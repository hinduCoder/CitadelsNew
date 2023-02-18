using Citadels.Core.Characters;

namespace Citadels.Core.Actions;

internal interface ICharacterAction : IAction
{
    void Execute(Game game, Character character);
}
