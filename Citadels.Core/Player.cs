using Citadels.Core.Characters;
using Citadels.Core.Districts;
using Citadels.Core.Districts.Special;
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

    public int DistrictCountToChooseFrom => HasDistrictOfType<Observatory>() ? 3 : 2;
    public bool CanTakeAllDistricts => HasDistrictOfType<Library>();
    public int Score => GetScore();
    public bool CityBuilt => BuiltDistricts.Count >= 7;

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
    internal void FoldDistrict(District district) => _districts.Remove(district);

    internal void ClearDistricts() => _districts.Clear();

    internal void BuildDistrict(District district)
    {
        if (!_districts.Remove(district))
        {
            throw new ArgumentException("The player doesn't have such district", nameof(district));
        }
        Coins -= district.BuildPrice;
        _builtDistricts.Add(district);
    }

    internal void DestroyDistrict(District district)
    {
        _builtDistricts.Remove(district);
    }

    internal bool HasDistrictOfType<T>() where T : District
        => BuiltDistricts.OfType<Library>().Any();

    private int GetScore()
    {
        var score = BuiltDistricts.Sum(x => x.Points);
        if (CityBuilt)
        {
            score += 2;
        }
        if (HasDistrictOfType<MapRoom>())
        {
            score += Districts.Count;
        }
        if (HasDistrictOfType<ImperialTreasure>())
        {
            score += Coins;
        }
        score += GetBonusPointsForKindsCombination();
        return score;
    }

    private int GetBonusPointsForKindsCombination()
    {
        var totalKindsCount = Enum.GetValues<DistrictKind>().Length;
        var playerKindsCounts = BuiltDistricts.GroupBy(x => x.Kind).ToDictionary(x => x.Key, x => x.Count());
        var playerDifferentKindsCount = playerKindsCounts.Count;
        var allKinds = BuiltDistricts.Where(x => x.CountAsAnyTypeAtTheEnd);
        foreach (var x in allKinds)
        {
            if (playerKindsCounts[x.Kind]-- > 1)
            {
                playerDifferentKindsCount++;
            }
        }
        if (playerDifferentKindsCount >= totalKindsCount)
        {
            return 3;
        }
        return 0;
    }
}
