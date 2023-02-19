using Citadels.Core.Characters;
using Citadels.Core.Districts;
using Citadels.Core.Structure;
using System.Diagnostics.CodeAnalysis;

namespace Citadels.Core;

public class Game
{
    private readonly List<Player> _players = new ();

    public GameStatus Status { get; private set; } = GameStatus.NotStarted;
    public IReadOnlyList<Player> Players => _players;
    [AllowNull]
    public Draft Draft { get; private set; }
    [AllowNull]
    public Round CurrentRound { get; set; }
    [AllowNull]
    public Turn CurrentTurn => CurrentRound.CurrentTurn;
    [AllowNull]
    internal Deck<District> DistrictDeck { get; private set; }

    internal void StartGame(IEnumerable<Player> players,
        IEnumerable<District> randomizedDistricts)
    {
        _players.AddRange(players);
        SetCrownOwner(Players[0]);
        DistrictDeck = new Deck<District>(randomizedDistricts);
        Status = GameStatus.ReadyToDraft;
    }

    internal void StartDraft(IEnumerable<Character> randomizedCharacters)
    {
        Draft = new Draft(randomizedCharacters, _players, _players.FindIndex(x => x.IsCrownOwner));
        Status = GameStatus.Draft;
    }

    internal void StartRound()
    {
        foreach (var player in Players)
        {
            player.AddDistricts(DistrictDeck.Take(4));
        }
        CurrentRound = new Round(this);
        Status = GameStatus.Round;
    }

    internal void SetCrownOwner(Player newCrownOwner)
    {
        foreach (var player in Players)
        {
            player.IsCrownOwner = false;
        }
        newCrownOwner.IsCrownOwner = true;
    } 
}
