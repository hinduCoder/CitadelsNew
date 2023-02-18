namespace Citadels.Core.Actions;

internal class PossibleAction<T> : IPossibleAction
    where T : class, IAction
{
    public bool Obligatory { get; }
    private PossibleAction(bool obligatory)
    {
        Obligatory = obligatory;
    }
    public bool SupportsAction(Type actionType)
        => actionType == typeof(T);

    internal static PossibleAction<T> Create(bool obligatory = false) 
    {
        return new PossibleAction<T>(obligatory);
    }
}
