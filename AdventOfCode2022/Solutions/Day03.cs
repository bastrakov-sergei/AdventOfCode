using System;
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
        
        TestContext.WriteLine($@"{part.GetDescription()}: {part switch
            {
                Part.Part1 => Solution1Solve(input),
                Part.Part2 => Solution2Solve(input),
                _ => throw new ArgumentOutOfRangeException(nameof(part), part, null),
            }
        }");
    }

    private static int Solution1Solve(IEnumerable<string> backpacks)
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

    private static int Solution2Solve(IEnumerable<string> backpacks)
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