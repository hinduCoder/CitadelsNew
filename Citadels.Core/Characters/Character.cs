using System.Reflection;
using Citadels.Core.Actions;

namespace Citadels.Core.Characters;

public abstract class Character : IComparable<Character>, IEquatable<Character>
{ 
    public static readonly Character[] Pool;
    static Character()
    {
        var characterTypes = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.BaseType == typeof(Character)).ToList();
        Pool = characterTypes.Select(characterType => (Character)Activator.CreateInstance(characterType)!).OrderBy(x => x.Rank).ToArray();
    }
    internal virtual IReadOnlyCollection<IPossibleAction> AvailableActions { get; } = Array.Empty<IPossibleAction>();
    internal virtual IReadOnlyCollection<ISimpleAction> AutomaticActions { get; } = Array.Empty<ISimpleAction>();

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
}
