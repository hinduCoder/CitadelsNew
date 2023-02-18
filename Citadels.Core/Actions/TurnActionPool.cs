namespace Citadels.Core.Actions;

internal class TurnActionPool
{
    private readonly List<Item> _actionsDone = new();

    internal TurnActionPool(IEnumerable<IPossibleAction> possibleActions)
    {
        _actionsDone.AddRange(possibleActions.Select(x => new Item(x)));
    }

    internal void MarkActionDone<T>() where T : class, IAction
    {
        MarkActionDone(typeof(T));
    }

    internal void MarkActionDone(Type actionType)
    {
        var item = _actionsDone.Find(x => x.PossibleAction.SupportsAction(actionType));
        if (item is null)
        {
            return;
        }
        item.Done = true;
    }

    internal bool AllObligatoryActionsDone => _actionsDone.Where(x => x.PossibleAction.Obligatory).All(x => x.Done);

    internal bool IsActionAvailable<T>() where T : IAction
    {
        var item = _actionsDone.Find(x => x.PossibleAction.SupportsAction(typeof(T)));
        if (item is null)
        {
            return false;
        }
        return !item.Done;
    }

    private record Item(IPossibleAction PossibleAction)
    {
        public bool Done { get; set; }
    }
}