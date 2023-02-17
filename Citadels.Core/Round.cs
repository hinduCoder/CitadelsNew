using Citadels.Core.Characters;
using System.Diagnostics.CodeAnalysis;

namespace Citadels.Core;

public class Round
{
    private readonly Game _game;
    private readonly List<(Character Character, Player Player)> _playersOrder = new();
    private int _turnNumber = 0;

    public Turn CurrentTurn { get; private set; }

    internal Round(Game game)
    {
        _game = game;
        _playersOrder.AddRange(_game.Players.Select(x => (Character: x.CurrentCharacter!, x)).OrderBy(x => x.Character));
        _playersOrder.ForEach(x => x.Player.IsAlive = true);
        NewTurn();
    }

    [MemberNotNull(nameof(CurrentTurn))]
    internal void NewTurn()
    {
        CurrentTurn = new Turn(_game, _playersOrder[_turnNumber].Player);
        _turnNumber++;
    }
}
