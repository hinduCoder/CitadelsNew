using Citadels.Core.Districts;
using Citadels.Core.Districts.Special;
using System.Diagnostics.CodeAnalysis;

namespace Citadels.Core;

public class Scoreboard
{
    private readonly List<Score> _scores = new();
    private Game Game { get; }

    public IReadOnlyList<Score> Scores => _scores;
    public Player Winner { get; private set; }

    internal Scoreboard(Game game) 
    {
        Game = game;
        CalculateScore();
    }

    [MemberNotNull(nameof(Winner))]
    private void CalculateScore()
    {
        foreach (var player in Game.Players)
        {
            var playerScore = player.Score;
            if (player == Game.CurrentRound.FirstPlayerWithBuiltCity)
            {
                playerScore += 2;
            }
            
            _scores.Add(new Score(player, playerScore));
        }

        Winner = Scores
            .OrderByDescending(x => x.Points)
            .ThenByDescending(x => x.Player.CurrentCharacter.Rank)
            .First().Player;
    }
}

public record Score(Player Player, int Points);
