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

    public override async Task<NewGameResponse> StartNewGame(NewGameRequest request, ServerCallContext context)
    {
        var newId = request.GameId;
        if (string.IsNullOrEmpty(newId))
        {
            newId = Guid.NewGuid().ToString();
        }

        using var gameAccessor = await _gameStorage.GetAsync(newId, context.CancellationToken);
        _gameEventHandler.HandleEvent(gameAccessor.Game,
            new StartGame(request.PlayerNames, (int)(DateTime.Now.Ticks % int.MaxValue)));
        return new NewGameResponse { Id = newId };
    }

    public override async Task<DraftStateResponse> StartDraft(StartDraftRequest request, ServerCallContext context)
    {
        using var gameAccessor = await _gameStorage.GetAsync(request.GameId, context.CancellationToken);
        _gameEventHandler.HandleEvent(gameAccessor.Game, new StartDraft((int)(DateTime.Now.Ticks % int.MaxValue)));
        _gameEventHandler.HandleEvent(gameAccessor.Game, new DiscardDraftFirstCards());
        return MapDraftState(gameAccessor.Game);
    }

    public override async Task<DraftStateResponse> ChooseCharacter(ChooseCharacterRequest request,
        ServerCallContext context)
    {
        using var gameAccessor = await _gameStorage.GetAsync(request.GameId, context.CancellationToken);
        _gameEventHandler.HandleEvent(gameAccessor.Game, new ChooseCharacter(request.Rank));
        return MapDraftState(gameAccessor.Game);
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