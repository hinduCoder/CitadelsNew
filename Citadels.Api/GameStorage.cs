using Citadels.Core;
using System.Collections.Concurrent;

namespace Citadels.Api;

internal class GameStorage : IGameStorage
{
    private readonly ConcurrentDictionary<Guid, DefaultGameAccessor> _games = new();

    public async Task<IGameAccessor> GetAsync(Guid id, CancellationToken ct = default)
    {
        var gameAccessor = _games.GetOrAdd(id, _ => new DefaultGameAccessor(new Game()));
        await gameAccessor.Lock.WaitAsync(ct);
        return gameAccessor;
    }

    private class DefaultGameAccessor : IGameAccessor
    {
        public Game Game { get; }
        internal readonly SemaphoreSlim Lock = new(1);

        public DefaultGameAccessor(Game game)
        {
            Game = game;
        }

        public void Dispose()
        {
            Lock.Release();
        }
    }
}