namespace Citadels.Core;

public interface IGameEvent
{
    bool IsValid(Game game);
    void Handle(Game game);
}
