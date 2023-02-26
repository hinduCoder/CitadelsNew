using Citadels.Api;
using Citadels.Core;
using Citadels.Core.Events;
using Grpc.Core;

namespace Citadels.Api.Services;
public class CitadelsService : Citadels.CitadelsBase
{
    private readonly ILogger<CitadelsService> _logger;
    private readonly IGameHostStorage _gameHostStorage;

    public CitadelsService(ILogger<CitadelsService> logger,
        IGameHostStorage gameHostStorage)
    {
        _logger = logger;
        _gameHostStorage = gameHostStorage;
    }

    public override async Task<NewGameResponse> StartNewGame(NewGameRequest request, ServerCallContext context)
    {
        var newId = request.GameId;
        if (string.IsNullOrEmpty(newId))
        {
            newId = Guid.NewGuid().ToString();
        }

        var host = _gameHostStorage[newId];

        await host.EnqueueAsync(new StartGame(request.PlayerNames, (int)(DateTime.Now.Ticks % int.MaxValue)));
        
        return new NewGameResponse { Id = newId };
    }

    public override async Task<DraftStateResponse> StartDraft(StartDraftRequest request, ServerCallContext context)
    {
        var host = _gameHostStorage[request.GameId];
        await host.EnqueueAsync(new StartDraft((int)(DateTime.Now.Ticks % int.MaxValue)));
        await host.EnqueueAsync(new DiscardDraftFirstCards());
        return MapDraftState(host.Game); //вот тут будет плохо. Игра еще может не поменяться, у нас все асинхронное
    }

    public override async Task<DraftStateResponse> ChooseCharacter(ChooseCharacterRequest request, ServerCallContext context)
    {
        var gameHost = _gameHostStorage[request.GameId];
        await gameHost.EnqueueAsync(new ChooseCharacter(request.Rank));
        return MapDraftState(gameHost.Game);
    }

    private static DraftStateResponse MapDraftState(Game game)
    {
        var result = new DraftStateResponse()
        {
            InProgress = !game.Draft.Completed,
            PlayerName = game.Draft.CurrentPlayer?.Name ?? string.Empty
        };
        result.AvailableRanks.AddRange(game.Draft.AvailableCharacters.Select(x => x.Rank));
        result.DiscardedRanks.AddRange(game.Draft.OpenDiscardedCharacters.Select(x => x.Rank));

        return result;
    }
}
