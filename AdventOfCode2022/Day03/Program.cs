using AOCCommon;

var backpacks = await File.ReadAllLinesAsync("input.txt");
var solution1 = Solution1Solve();
var solution2 = Solution2Solve();

Console.WriteLine($"Solution 1: {solution1}");
Console.WriteLine($"Solution 2: {solution2}");


int Solution1Solve()
{
    var totalSum = 0;
    foreach (var backpack in backpacks)
    {
        var left = backpack[..(backpack.Length / 2)];
        var right = backpack[(backpack.Length / 2)..];

        var commonItem = left.Intersect(right).Single();
        totalSum += char.IsUpper(commonItem)
            ? commonItem - 'A' + 27
            : commonItem - 'a' + 1;
    }

    return totalSum;
}

int Solution2Solve()
{
    var totalSum = 0;
    foreach (var backpackSet in backpacks.Batch(3))
    {
        var backpackArray = backpackSet.ToArray();
        var first = backpackArray[0];
        var second = backpackArray[1];
        var third = backpackArray[2];
        
        var commonItem = first.Intersect(second).Intersect(third).Single();
        totalSum += char.IsUpper(commonItem)
            ? commonItem - 'A' + 27
            : commonItem - 'a' + 1;
    }

    return totalSum;
}