using System.Reflection;

namespace Citadels.Core.Characters;

//Old copy
public abstract class Character
{
    public static Character[] Pool;
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
}
