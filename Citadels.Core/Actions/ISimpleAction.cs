namespace Citadels.Core.Actions;

internal interface ISimpleAction : IAction
{
    void Execute(Game game);
}

internal interface ISimpleAction<TParam> : IAction
{
    void Execute(Game game, TParam param);
}
