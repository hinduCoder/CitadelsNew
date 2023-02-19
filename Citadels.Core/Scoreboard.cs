using Citadels.Core.Districts;
using System.Diagnostics.CodeAnalysis;

namespace Citadels.Core;

public class Scoreboard
{
    private readonly List<Score> _scores = new List<Score>();
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
            var playerScore = player.BuiltDistricts.Sum(x => x.BuildPrice);
            if (player.BuiltDistricts.Select(x => x.Kind).Distinct().Count() == Enum.GetValues<DistrictKind>().Length)
            {
                playerScore += 3;
            }
            if (player.Name == Game.CurrentRound.FirstPlayerWithBuiltCity?.Name)
            {
                playerScore += 2;
            }
            if (player.BuiltDistricts.Count >= 7)
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
