var file = await File.ReadAllLinesAsync("input.txt");

var solution1 = Solution1Solve();
var solution2 = Solution2Solve();

Console.WriteLine($"Solution 1: {solution1}");
Console.WriteLine($"Solution 2: {solution2}");

int Solution1Solve()
{
    var count = 0;
    foreach (var line in file)
    {
        var pairs = line.Split(",");
        var first = ParseRange(pairs[0]);
        var second = ParseRange(pairs[1]);

        if (first.Contains(second) || second.Contains(first))
        {
            count++;
        }
    }

    return count;
}

int Solution2Solve()
{
    var count = 0;
    foreach (var line in file)
    {
        var pairs = line.Split(",");
        var first = ParseRange(pairs[0]);
        var second = ParseRange(pairs[1]);

        if (first.Overlaps(second))
        {
            count++;
        }
    }

    return count;
}


Range ParseRange(string input)
{
    var parts = input.Split("-");
    return new Range(int.Parse(parts[0]), int.Parse(parts[1]));
}

public record Range(int Left, int Right)
{
    public bool Contains(Range other)
        => Left <= other.Left && Right >= other.Right;

    public bool Overlaps(Range other) 
        => other.Left <= Right && Left <= other.Right || 
           Left <= other.Right && other.Left <= Right;
}