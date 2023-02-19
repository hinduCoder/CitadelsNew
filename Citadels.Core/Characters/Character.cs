using System.Reflection;
using Citadels.Core.Actions;

namespace Citadels.Core.Characters;

public abstract class Character : IComparable<Character>, IEquatable<Character>
{ 
    public static readonly Character[] Pool;
    static Character()
    {
        var characterTypes = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.BaseType == typeof(Character)).ToList();
        Pool = characterTypes.Select(characterType => (Character)Activator.CreateInstance(characterType, nonPublic: true)!).OrderBy(x => x.Rank).ToArray();
    }

    private protected List<IPossibleAction> _availableActions = new();
    private protected List<ISimpleAction> _automaticActions = new();
    internal IReadOnlyCollection<IPossibleAction> AvailableActions => _availableActions;
    internal IReadOnlyCollection<ISimpleAction> AutomaticActions => _automaticActions;

    public abstract int Rank { get; }
    public virtual int DistrictMaxBuildCount { get; } = 1;
    public virtual bool CanDistrictsBeDesctroyed { get; } = true;

    public string Name => ToString();

    public bool Is<TCharacter>() where TCharacter: Character => this is TCharacter;

    public override string ToString()
    {
        return GetType().Name;
    }

    int IComparable<Character>.CompareTo(Character? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Rank - other.Rank;
    }

    public bool Equals(Character? other) => Rank == other?.Rank;

    public override bool Equals(object? obj)
    {
        if (obj is not Character other)
        {
            return false;
        }
        return Equals(other);
    }

    public override int GetHashCode() => Rank;

    private protected void RegisterAutomaticActions(params ISimpleAction[] actions)
        => _automaticActions.AddRange(actions);

    private protected void RegisterPossibleActions(params IPossibleAction[] possibleActions)
        => _availableActions.AddRange(possibleActions);
}
