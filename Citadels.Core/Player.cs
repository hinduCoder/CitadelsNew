using Citadels.Core.Characters;
using Citadels.Core.Districts;

namespace Citadels.Core;

//Old copy
public class Player
{
    private readonly string _name;
    private int _coins;
    private Character? _currentCharacter;
    private List<District> _districts = new();
    private List<District> _builtDistricts = new();

    public string Name => _name;
    public int Coins
    {
        get => _coins;
        internal set => _coins = value;
    }
    public Character? CurrentCharacter
    {
        get => _currentCharacter;
        internal set => _currentCharacter = value;
    }
    public IReadOnlyList<District> Districts => _districts;
    public IEnumerable<District> BuiltDistricts => _builtDistricts;

    public Player(string name)
    {
        _name = name;
        _coins = 2;
    }

    internal void AddDistricts(IEnumerable<District> districts)
    {
        foreach (var district in districts)
        {
            AddDistrict(district);
        }
    }
    internal void AddDistrict(District district) => _districts.Add(district);
    internal void RemoveDistrict(District district) => _districts.Remove(district);
    internal IReadOnlyList<District> SwapDistricts(IEnumerable<District> districts)
    {
        var result = _districts.ToList();
        _districts.Clear();
        AddDistricts(districts);
        return result;
    }

    internal void BuildDistrict(District district)
    {
        _builtDistricts.Add(district);
        _districts.Remove(district);
        _coins -= district.BuildPrice;
    }

    internal void AddCoinsForEveryBuiltDistrictOfKind(DistrictKind districtKind)
    {
        Coins += BuiltDistricts.Count(d => d.Kind == districtKind);
    }
}
