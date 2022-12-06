using System;
using System.Collections.Generic;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022;

[Ignore("Template")]
public class DaySolutionTemplate : AdventOfCodeBaseSolution
{
    public DaySolutionTemplate() : base(5)
    {
    }

    [TestCase(Part.Part1, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, TestName = "Part 2 solution")]
    public void Solve(Part part)
    {
        var input = Input.ReadAllLines();
        PrintSolution(part, () => SolvePart1(input), () => SolvePart2(input));
    }

    private static string SolvePart1(IEnumerable<string> lines)
    {
        throw new NotImplementedException();
    }

    private static string SolvePart2(IEnumerable<string> lines)
    {
        throw new NotImplementedException();
    }
}