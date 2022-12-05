namespace AOCCommon;

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T>? items, int maxItems)
        => (items ?? Enumerable.Empty<T>())
            .Select((item, inx) => new { item, inx, })
            .GroupBy(x => x.inx / maxItems)
            .Select(g => g.Select(x => x.item));
}