namespace Citadels.Utils;

public static class ListExtensions
{
    public static IList<T> Shuffle<T>(this IList<T> list, int? randomSeed = null)
    {
        var random = randomSeed.HasValue ? new Random(randomSeed.Value) : Random.Shared;
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
        return list;
    }
}