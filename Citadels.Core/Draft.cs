using Citadels.Core.Characters;

namespace Citadels.Core;

public class Draft
{
    private const int MaximumPlayersCount = 7;

    private readonly List<Character> _characters = new();
    private readonly List<Character> _openDiscardedCharacters = new();

    private readonly List<Player> _players = new();
    private int _currentPlayerIndex = 0;

    public int PlayersCount => _players.Count;
    public bool Started => _currentPlayerIndex != 0 || ClosedDiscardedCharacter != null;
    public bool Completed => _currentPlayerIndex == _players.Count;
    public IReadOnlyList<Character> AvailableCharacters => _characters;
    public IReadOnlyList<Character> OpenDiscardedCharacters => _openDiscardedCharacters;
    public Character? ClosedDiscardedCharacter { get; private set; }


    public Draft(IEnumerable<Character> randomizedCharacters, IReadOnlyList<Player> players, int firstPlayer)
    {
        _characters.AddRange(randomizedCharacters);

        _players.AddRange(players.Skip(firstPlayer));
        _players.AddRange(players.Take(players.Count - PlayersCount));
    }

    public void DiscardFirstCards()
    {
        if (Started)
        {
            throw new InvalidOperationException();
        }
        var openCardsCount = Math.Max(7 - _players.Count - 1, 0);
        _openDiscardedCharacters.AddRange(_characters.Take(openCardsCount));
        _characters.RemoveRange(0, openCardsCount);
        ClosedDiscardedCharacter = _characters[0];
        _characters.RemoveAt(0);

        _characters.Sort();
    }

    public void ChooseCharacterForCurrentPlayer(int rank)
    {
        var character = _characters.Find(x => x.Rank == rank);
        if (character is null)
        {
            throw new InvalidOperationException();
        }
        _players[_currentPlayerIndex++].CurrentCharacter = character;
        _characters.Remove(character);

        if (PlayersCount == MaximumPlayersCount && _currentPlayerIndex == MaximumPlayersCount - 1)
        {
            _characters.Add(ClosedDiscardedCharacter!);
            ClosedDiscardedCharacter = null;
        }
    }
}
