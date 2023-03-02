using Citadels.Core;

namespace Citadels.Api;

public interface IGameAccessor : IDisposable
{
    Game Game { get; }
}