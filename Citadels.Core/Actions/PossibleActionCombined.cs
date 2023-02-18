namespace Citadels.Core.Actions;

internal class PossibleActionCombined<T1, T2> : IPossibleAction
    where T1 : IAction
    where T2 : IAction
{
    public bool Obligatory { get; }
    private PossibleActionCombined(bool obligatory)
    {
        Obligatory = obligatory;
    }
    public bool SupportsAction(Type actionType)
        => actionType == typeof(T1) || actionType == typeof(T2);

    internal static PossibleActionCombined<T1, T2> Create(bool obligatory = false)
    {
        return new PossibleActionCombined<T1, T2>(obligatory);
    }
}
