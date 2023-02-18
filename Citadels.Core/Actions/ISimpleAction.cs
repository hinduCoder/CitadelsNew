namespace Citadels.Core.Actions;

internal interface ISimpleAction : IAction
{
    void Execute(Game game);
}
