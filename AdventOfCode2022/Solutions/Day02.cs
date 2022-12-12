using System.Collections.Generic;
using System.Linq;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022.Solutions;

public class Day02 : AdventOfCodeBaseSolution
{
    public Day02() : base()
    {
    }

    [TestCase(Part.Part1, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, TestName = "Part 2 solution")]
    public void Solve(Part part)
    {
        var input = Input
            .ReadAllLines()
            .Select(line => (op: line[0] - 'A', me: line[2] - 'X'))
            .ToArray();

        PrintSolution(part, () => SolvePart1(input), () => SolvePart2(input));
    }

    private static string SolvePart1(IEnumerable<(int op, int me)> input)
    {
        var score = new[,]
        {
            { 4, 1, 7 },
            { 8, 5, 2 },
            { 3, 9, 6 },
        };

        return input.Select(turn => score[turn.me, turn.op]).Sum().ToString();
    }

    private static string SolvePart2(IEnumerable<(int op, int me)> input)
    {
        var score = new[,]
        {
            { 3, 1, 2 },
            { 4, 5, 6 },
            { 8, 9, 7 },
        };

        return input.Select(turn => score[turn.me, turn.op]).Sum().ToString();
    }
}