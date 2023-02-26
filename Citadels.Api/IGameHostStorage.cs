using Citadels.Core;

namespace Citadels.Api;
public interface IGameHostStorage
{
    GameHost this[Guid id] { get; }
    GameHost this[string guid] => Guid.TryParse(guid, out var gameGuid) ? this[gameGuid] : throw new FormatException();
}