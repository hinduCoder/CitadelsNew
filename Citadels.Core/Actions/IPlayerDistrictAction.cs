using Citadels.Core.Districts;

namespace Citadels.Core.Actions;

internal interface IPlayerDistrictAction : IAction
{
    void Execute(Game game, Player player, District district);
}
