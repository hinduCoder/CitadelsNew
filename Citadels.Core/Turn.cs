using Citadels.Core.Characters;
using Citadels.Core.Districts;

namespace Citadels.Core;

public class Turn
{
    private readonly List<District> _districtsForChoose = new ();
    private readonly Game _game;

    public Player Player { get; private set; }
    public bool GatherActionDone { get; private set; }
    public bool GatherActionInProgress { get; private set; }
    public bool CharacterActionDone { get; private set; }
    public int DistrictBuiltCount { get; private set; }

    public IReadOnlyList<District> DistrictsForChoose => _districtsForChoose;
    public bool GatherActionAvailable => !GatherActionDone && !GatherActionInProgress;
    public bool CanBuild => DistrictBuiltCount < Player.CurrentCharacter.DistrictMaxBuildCount;

    internal Turn(Game game, Player player)
    {
        _game = game;
        Player = player;
    }

    internal void GatherCoins()
    {
        Player.Coins += 2;
        GatherActionDone= true;
    }

    internal void GatherDistrict()
    {
        _districtsForChoose.AddRange(_game.DistrictDeck.Take(2));
        GatherActionInProgress= true;
    }

    internal void ChooseDistrict(District district)
    {
        if (!DistrictsForChoose.Contains(district))
        {
            throw new ArgumentException("District is not in the allowed list", nameof(district));
        }

        Player.AddDistrict(district);
        GatherActionInProgress = false;
        GatherActionDone = true;
    }

    internal void BuildDistrict(District district)
    {
        Player.BuildDistrict(district);
        DistrictBuiltCount++;
    }
}
