using Citadels.Core.Errors;

namespace Citadels.Core;

public interface IGameEventV2
{
    void Handle(Game game);
    //думаю лучше использовать этот контракт, чтобы еще можно было пробрасывать ошибку - что именно не так.
    // на фронте будет полезно
    (bool isValid, Error? error) IsValid(Game game);
}
