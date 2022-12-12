using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022.Solutions;

public class Day09 : AdventOfCodeBaseSolution
{
    public Day09() : base()
    {
    }

    [TestCase(Part.Part1, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, TestName = "Part 2 solution")]
    public void Solve(Part part)
    {
        var input = Input.ReadAllLines().ToArray();
        PrintSolution(part, () => Solve(input, 2), () => Solve(input, 10));
    }

    [TestCase(ExpectedResult = "13")]
    public string SolveTestPart1()
    {
        var input = "R 4\nU 4\nL 3\nD 1\nR 4\nD 1\nL 5\nR 2".Split('\n');
        return Solve(input, 2);
    }

    [TestCase(ExpectedResult = "36")]
    public string SolveTestPart2()
    {
        var input = "R 5\nU 8\nL 8\nD 3\nR 17\nD 10\nL 25\nU 20".Split('\n');
        return Solve(input, 10);
    }

    private static string Solve(string[] input, int count)
    {
        var moves = input
            .Select(line => line.Split())
            .Select(words => new Move(Directions[words[0]], int.Parse(words[1])))
            .ToArray();

        var rope = moves.Aggregate(Rope.Build(count), (s, move) => s.Apply(move));

        /*TestContext.WriteLine("=== Visited ===");
        foreach (var vector in rope.Visited)
        {
            TestContext.WriteLine($"{vector}");
        }*/

        return rope.Visited.Count.ToString();
    }

    public record Move(Vector Direction, int Steps);

    public record Rope(ImmutableArray<Vector> Knots, ImmutableHashSet<Vector> Visited)
    {
        public Rope Apply(Move move)
            => Apply(this, move);

        private static Rope Apply(Rope rope, Move move)
        {
            for (var i = 0; i < move.Steps; i++)
            {
                var knots = rope.Knots.SetItem(0, rope.Knots[0] + move.Direction);

                for (var j = 0; j < rope.Knots.Length - 1; j++)
                {
                    if ((knots[j] - knots[j + 1]).SqrLength > 2)
                    {
                        knots = knots.SetItem(j + 1, knots[j + 1] + (knots[j] - knots[j + 1]).Direction);
                    }
                }

                rope = new Rope(knots, rope.Visited.Add(knots[^1]));
            }

            return rope;
        }

        public static Rope Build(int knotsCount)
            => new(Enumerable.Range(0, knotsCount).Select(_ => Vector.Zero).ToImmutableArray(),
                ImmutableHashSet<Vector>.Empty.Add(Vector.Zero));
    }

    private static readonly Dictionary<string, Vector> Directions = new()
    {
        { "U", Vector.Up },
        { "D", Vector.Down },
        { "R", Vector.Right },
        { "L", Vector.Left },
    };
}