using Citadels.Core.Characters;
using Citadels.Core.Districts;

namespace Citadels.Core;

public class Player
{
    private readonly List<District> _districts = new();
    private readonly List<District> _builtDistricts = new();

    public string Name { get; }
    public int Coins { get; internal set; }
    public Character? CurrentCharacter { get; internal set; }

    public IReadOnlyList<District> Districts => _districts;
    public IEnumerable<District> BuiltDistricts => _builtDistricts;

    public Player(string name)
    {
        Name = name;
        Coins = 2;
    }

    public override string ToString() => Name;
}
