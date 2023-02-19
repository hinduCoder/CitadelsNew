using Citadels.Core.Characters;
using Citadels.Utils;

namespace Citadels.Core.Events;

public class StartDraft : IGameEvent
{
    public int RandomSeed { private set; get; }

    public StartDraft(int randomSeed)
    {
        RandomSeed = randomSeed;
    }

    public void Handle(Game game)
    {
        var characterPool = Character.Pool.ToList();
        characterPool.Shuffle(RandomSeed);
        game.StartDraft(characterPool);
    }

    public bool IsValid(Game game)
        => game.Status == GameStatus.ReadyToDraft;
}
