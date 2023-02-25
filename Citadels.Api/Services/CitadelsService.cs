using Citadels.Api;
using Citadels.Core;
using Citadels.Core.Events;
using Grpc.Core;

namespace Citadels.Api.Services;
public class CitadelsService : Citadels.CitadelsBase
{
    private readonly ILogger<CitadelsService> _logger;
    private readonly GameEventHandler _gameEventHandler;
    private readonly IGameStorage _gameStorage;

    public CitadelsService(ILogger<CitadelsService> logger,
        GameEventHandler gameEventHandler,
        IGameStorage gameStorage)
    {
        _logger = logger;
        _gameEventHandler = gameEventHandler;
        _gameStorage = gameStorage;
    }

    public override Task<NewGameResponse> StartNewGame(NewGameRequest request, ServerCallContext context)
    {
        var newId = request.GameId;
        if (string.IsNullOrEmpty(newId))
        {
            newId = Guid.NewGuid().ToString();
        }

        _gameEventHandler.HandleEvent(_gameStorage[newId], new StartGame(request.PlayerNames, (int)(DateTime.Now.Ticks % int.MaxValue)));
        return Task.FromResult(new NewGameResponse { Id = newId });
    }

    public override Task<DraftStateResponse> StartDraft(StartDraftRequest request, ServerCallContext context)
    {
        Game game = _gameStorage[request.GameId];
        _gameEventHandler.HandleEvent(game, new StartDraft((int)(DateTime.Now.Ticks % int.MaxValue)));
        _gameEventHandler.HandleEvent(game, new DiscardDraftFirstCards());
        return Task.FromResult(MapDraftState(game));
    }

    public override Task<DraftStateResponse> ChooseCharacter(ChooseCharacterRequest request, ServerCallContext context)
    {
        var game = _gameStorage[request.GameId];
        _gameEventHandler.HandleEvent(game, new ChooseCharacter(request.Rank));
        return Task.FromResult(MapDraftState(game));
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
