namespace Citadels.Core.Events;

public class ChooseCharacter : IGameEvent
{
    public int SelectedCharacterRank { get; }
    public ChooseCharacter(int selectedCharacterRank)
    {
        SelectedCharacterRank = selectedCharacterRank;
    }

    public void Handle(Game game)
    {
        game.Draft.ChooseCharacterForCurrentPlayer(SelectedCharacterRank);
    }

    public bool IsValid(Game game)
    {
        if (game.Status != GameStatus.Draft)
        {
            return false;
        }
        var draft = game.Draft;
        return draft.Started
                && !draft.Completed
                && draft.AvailableCharacters.Any(x => x.Rank == SelectedCharacterRank);
    }
}
