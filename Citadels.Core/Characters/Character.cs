using System.Reflection;

namespace Citadels.Core.Characters;

//Old copy
public abstract class Character : IComparable<Character>, IEquatable<Character>
{ 
    public static readonly Character[] Pool;
    static Character()
    {
        var characterTypes = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.BaseType == typeof(Character)).ToList();
        Pool = characterTypes.Select(characterType => (Character)Activator.CreateInstance(characterType)!).OrderBy(x => x.Rank).ToArray();
    }
    public abstract int Rank { get; }
    public virtual int DistrictMaxBuildCount { get; } = 1;
    public virtual bool CanDistrictsBeDesctroyed { get; } = true;

    public bool IsAlive { get; internal set; } = true;

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

    bool IEquatable<Character>.Equals(Character? other) => Rank == other?.Rank;

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
