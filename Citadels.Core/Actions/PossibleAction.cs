namespace Citadels.Core.Actions;

internal class PossibleAction<T> : IPossibleAction
    where T : class, IAction
{
    public bool Obligatory { get; }
    internal PossibleAction(T action)
    {
        Obligatory = action.Obligatory;
    }
    public bool SupportsAction(Type actionType)
        => actionType == typeof(T);
}
