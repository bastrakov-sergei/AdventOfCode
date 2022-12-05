using System;
using System.Collections.Generic;
using System.Linq;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022.Solutions;

public class Day01 : AdventOfCodeBaseSolution
{
    public Day01() : base(1)
    {
    }

    [TestCase(Part.Part1, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, TestName = "Part 2 solution")]
    public void Solve(Part part)
    {
        var caloriesPerElve = SelectTotalCaloriesPerElve(Input.ReadAllLines()).ToArray();
        TestContext.WriteLine(
            $@"{part.GetDescription()}: {part switch
                {
                    Part.Part1 => caloriesPerElve.Max(),
                    Part.Part2 => caloriesPerElve.OrderByDescending(i => i).Take(3).Sum(),
                    _ => throw new ArgumentOutOfRangeException(nameof(part), part, null)
                }
            }");
    }

    private static IEnumerable<int> SelectTotalCaloriesPerElve(IEnumerable<string> lines)
    {
        var sum = 0;
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                yield return sum;
                sum = 0;
            }
            else
            {
                sum += int.Parse(line);
            }
        }
    }
}