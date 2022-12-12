using System.ComponentModel;

namespace AOCCommon;

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T>? items, int maxItems)
        => (items ?? Enumerable.Empty<T>())
            .Select((item, inx) => new { item, inx, })
            .GroupBy(x => x.inx / maxItems)
            .Select(g => g.Select(x => x.item));

    public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
    {
        first = (list.Count > 0 ? list[0] : default) ?? throw new InvalidOperationException();
        rest = list.Skip(1).ToList();
    }

    public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
    {
        first = (list.Count > 0 ? list[0] : default) ?? throw new InvalidOperationException();
        second = (list.Count > 1 ? list[1] : default) ?? throw new InvalidOperationException();
        rest = list.Skip(2).ToList();
    }

    public static string GetDescription(this Enum value)
    {
        if (Enum
            .GetValues(value.GetType())
            .Cast<Enum>()
            .All(x => !Equals(x, value)))
        {
            return value.ToString();
        }

        var field = value.GetType().GetField(value.ToString());
        return Attribute.GetCustomAttribute(
            field,
            typeof(DescriptionAttribute)) is not DescriptionAttribute attribute
            ? value.ToString()
            : attribute.Description;
    }

    public static IEnumerable<string> ReadAllLines(this StreamReader reader)
        => reader.ReadAllLines<string>();

    public static IEnumerable<T> ReadAllLines<T>(this StreamReader reader)
    {
        var list = new List<T>();
        while (!reader.EndOfStream)
        {
            list.Add((T)Convert.ChangeType(reader.ReadLine()!, typeof(T)));
        }

        return list.ToArray();
    }

    public static IEnumerable<T[]> SplitBy<T>(this IEnumerable<T> elements, Predicate<T> predicate)
    {
        var chunk = new List<T>();
        foreach (var element in elements)
        {
            if (predicate(element))
            {
                yield return chunk.ToArray();
                chunk.Clear();
                continue;
            }

            chunk.Add(element);
        }

        if (chunk.Any())
        {
            yield return chunk.ToArray();
        }
    }
}