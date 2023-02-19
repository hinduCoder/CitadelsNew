using Citadels.Core.Actions.DistrictActions;

namespace Citadels.Core.Events;

public class RestoreDestroyedDistrict : IGameEvent
{
    public Player TargetPlayer { get; set; }

    public RestoreDestroyedDistrict(Player targetPlayer)
    {
        TargetPlayer = targetPlayer;
    }

    public void Handle(Game game)
    {
        game.CurrentTurn.ExecuteAction(new RestoreDestroyedDistrictAction(), TargetPlayer);
    }

    public bool IsValid(Game game)
        => game is { Status: GameStatus.Round, CurrentTurn.LastDestroyedDistrict: not null }
        && game.CurrentTurn.ActionAvaialble<RestoreDestroyedDistrictAction>()
        && TargetPlayer.Coins >= 1;
}