using System.Threading.Channels;
using Citadels.Core;

namespace Citadels.Api;

public sealed class GameHost
{
    public Game Game { get; }
    private Task _listener;
    private readonly Channel<IGameEvent> _channel;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly GameEventHandler _gameEventHandler;

    public GameHost(Game game)
    {
        Game = game;
        _cancellationTokenSource = new CancellationTokenSource();
        _channel = Channel.CreateUnbounded<IGameEvent>();
        _listener = CrateListener(_cancellationTokenSource.Token);
        _gameEventHandler = new GameEventHandler();
    }

    public async Task EnqueueAsync(IGameEvent gameEvent, CancellationToken ct = default)
    {
        await _channel.Writer.WriteAsync(gameEvent, ct);
    }

    private async Task CrateListener(CancellationToken ct = default)
    {
        await foreach (var gameEvent in _channel.Reader.ReadAllAsync(ct))
        {
            _gameEventHandler.HandleEvent(Game, gameEvent);
        }
    }
}