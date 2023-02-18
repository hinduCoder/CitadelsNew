using Citadels.Core.Characters;
using Citadels.Core.Districts;
using System.Diagnostics.CodeAnalysis;

namespace Citadels.Core;

public class Player
{
    private readonly List<District> _districts = new();
    private readonly List<District> _builtDistricts = new();

    public string Name { get; }
    public int Coins { get; internal set; }
    [AllowNull]
    public Character CurrentCharacter { get; internal set; }
    public bool IsCrownOwner { get; internal set; }
    public bool IsAlive { get; internal set; } = true;

    public IReadOnlyList<District> Districts => _districts;
    public IReadOnlyList<District> BuiltDistricts => _builtDistricts;

    public Player(string name)
    {
        Name = name;
        Coins = 2;
    }

    public override string ToString() => Name;

    internal void AddDistricts(params District[] districts) => AddDistricts(districts.AsEnumerable());
    internal void AddDistricts(IEnumerable<District> districts) => _districts.AddRange(districts);

    internal void BuildDistrict(District district)
    {
        if (!_districts.Remove(district))
        {
            throw new ArgumentException("The player doesn't have such district", nameof(district));
        }
        Coins -= district.BuildPrice;
        _builtDistricts.Add(district);
    }
}
