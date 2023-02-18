namespace Citadels.Core.Actions;

internal interface IPossibleAction
{
    bool Obligatory { get; }
    bool SupportsAction(Type actionType);
}
