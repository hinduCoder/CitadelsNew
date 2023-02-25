using Citadels.Core;
using System.Collections.Concurrent;

namespace Citadels.Api;

internal class GameStorage : IGameStorage
{
    private readonly ConcurrentDictionary<Guid, Game> _games = new();
    public Game this[Guid id] => _games.GetOrAdd(id, newId => new Game());
}
