namespace Citadels.Core.Actions;

internal abstract class AutomaticAction : ISimpleAction
{
    public bool Obligatory => true;

    public abstract void Execute(Game game);
}
