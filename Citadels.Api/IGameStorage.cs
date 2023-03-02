using Citadels.Core;

namespace Citadels.Api;

public interface IGameStorage
{
    Task<IGameAccessor> GetAsync(Guid id, CancellationToken ct = default);
    Task<IGameAccessor> GetAsync(string guid, CancellationToken ct = default) =>
        Guid.TryParse(guid, out var gameGuid) ? GetAsync(gameGuid, ct) : throw new FormatException();
}