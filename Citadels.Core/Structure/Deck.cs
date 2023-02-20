using Citadels.Utils;

namespace Citadels.Core.Structure;

public class Deck<T>
{
    private readonly Queue<T> _cards;

    public Deck(IEnumerable<T> cards, bool shuffle = false)
    {
        if (shuffle)
        {
            cards = cards.ToArray().Shuffle();
        }

        _cards = new Queue<T>(cards);
    }

    public T? Take()
    {
        var list = Take(1);
        return list.Any() ? list[0] : default;
    }

    public IReadOnlyList<T> Take(int count)
    {
        var result = new List<T>();
        for (var i = 0; i < count && _cards.Count > 0; i++)
        {
            result.Add(_cards.Dequeue());
        }

        return result;
    }

    public void PutUnder(params T[] cards) => PutUnder(cards.AsEnumerable());

    public void PutUnder(IEnumerable<T> cards)
    {
        foreach (var card in cards)
        {
            _cards.Enqueue(card);
        }
    }
}