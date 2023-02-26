using Citadels.Core;
using System.Collections.Concurrent;

namespace Citadels.Api;

internal class GameHostStorage : IGameHostStorage
{
    private readonly ConcurrentDictionary<Guid, GameHost> _gameHosts = new();
    public GameHost this[Guid id] => _gameHosts.GetOrAdd(id, _ => new GameHost(new Game()));
}