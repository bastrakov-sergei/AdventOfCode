using System.Collections.Generic;
using System.Linq;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022.Solutions;

public class Day03 : AdventOfCodeBaseSolution
{
    public Day03() : base(3)
    {
    }

    [TestCase(Part.Part1, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, TestName = "Part 2 solution")]
    public void Solve(Part part)
    {
        var input = Input.ReadAllLines();

        PrintSolution(part, () => SolvePart1(input), () => SolvePart2(input));
    }

    private static int SolvePart1(IEnumerable<string> backpacks)
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

    private static int SolvePart2(IEnumerable<string> backpacks)
    {
        var totalSum = 0;
        foreach (var backpackSet in backpacks.Batch(3))
        {
            var backpackArray = backpackSet.ToArray();
            var commonItem = backpackArray[0].Intersect(backpackArray[1]).Intersect(backpackArray[2]).Single();
            totalSum += char.IsUpper(commonItem)
                ? commonItem - 'A' + 27
                : commonItem - 'a' + 1;
        }

        return totalSum;
    }
}