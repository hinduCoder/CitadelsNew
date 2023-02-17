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
    internal Deck<District> DistrictDeck { get; private set; }

    internal void StartGame(IEnumerable<Player> players, 
        IEnumerable<Character> randomizedCharacters,
        IEnumerable<District> randomizedDistricts)
    {
        _players.AddRange(players);
        Draft = new Draft(randomizedCharacters, _players, 0); //TODO crown owner index mustn't be always 0
        DistrictDeck = new Deck<District>(randomizedDistricts);

        Status = GameStatus.Draft;
    }

