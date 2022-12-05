using System;
using System.Collections.Generic;
using System.Linq;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022.Solutions;

public class Day04 : AdventOfCodeBaseSolution
{
    public Day04() : base(4)
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

    private static int Solution1Solve(IEnumerable<string> lines)
    {
        var count = 0;
        foreach (var line in lines)
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

    private static int Solution2Solve(IEnumerable<string> lines)
    {
        var count = 0;
        foreach (var line in lines)
        {
            var (first, second, _) = line.Split(",").Select(ParseRange).ToArray();
            if (first.Overlaps(second))
            {
                count++;
            }
        }

        return count;
    }

    private static Range ParseRange(string input)
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
}