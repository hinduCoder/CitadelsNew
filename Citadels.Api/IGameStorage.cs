using Citadels.Core;

namespace Citadels.Api;
public interface IGameStorage
{
    Game this[Guid id] { get; }
    Game this[string guid] => Guid.TryParse(guid, out var gameGuid) ? this[gameGuid] : throw new FormatException();
}