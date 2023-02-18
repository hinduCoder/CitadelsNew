using Citadels.Core.Characters;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Citadels.Core;

public class Round
{
    private readonly Game _game;
    private readonly List<Player> _playersOrder = new();
    private int _turnNumber = 0;

    public Turn CurrentTurn { get; private set; }

    internal event Action<Player, Character>? CharacterRevealEvent; 

    internal Round(Game game)
    {
        _game = game;
        _playersOrder.AddRange(_game.Players.OrderBy(x => x.CurrentCharacter));
        _playersOrder.ForEach(x => x.IsAlive = true);
        NewTurn();
    }

    [MemberNotNull(nameof(CurrentTurn))]
    internal void NewTurn()
    {
        CurrentTurn?.End();

        Player player;
        do
        {
            player = _playersOrder[_turnNumber++];
        } while (!player.IsAlive);
        CharacterRevealEvent?.Invoke(player, player.CurrentCharacter);

        CurrentTurn = new Turn(_game, player);
    }

    internal void End()
    {
        CharacterRevealEvent = null;
        var playerWithKing = _playersOrder.SingleOrDefault(x => x.CurrentCharacter.Is<King>());
        if (playerWithKing is not null)
        {
            _game.SetCrownOwner(playerWithKing);
        }
    }
}
