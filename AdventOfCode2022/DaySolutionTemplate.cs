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
        
        TestContext.WriteLine($@"{part.GetDescription()}: {part switch
            {
                Part.Part1 => Solution1Solve(input),
                Part.Part2 => Solution2Solve(input),
                _ => throw new ArgumentOutOfRangeException(nameof(part), part, null),
            }
        }");
    }

    private static string Solution1Solve(IEnumerable<string> lines)
    {
        throw new NotImplementedException();
    }

    private static string Solution2Solve(IEnumerable<string> lines)
    {
        throw new NotImplementedException();
    }
}