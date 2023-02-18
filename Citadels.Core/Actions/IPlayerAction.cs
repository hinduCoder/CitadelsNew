namespace Citadels.Core.Actions;

internal interface IPlayerAction : IAction
{
    void Execute(Game game, Player player);
}
