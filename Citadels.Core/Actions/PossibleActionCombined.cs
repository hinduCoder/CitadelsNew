namespace Citadels.Core.Actions;

internal class PossibleActionCombined<T1, T2> : IPossibleAction
    where T1 : IAction
    where T2 : IAction
{
    public bool Obligatory { get; }
    internal PossibleActionCombined(T1 action1, T2 action2)
    {
        Obligatory = action1.Obligatory && action2.Obligatory;
    }
    public bool SupportsAction(Type actionType)
        => actionType == typeof(T1) || actionType == typeof(T2);
}
