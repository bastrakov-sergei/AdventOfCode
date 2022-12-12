using System;
using System.Collections.Generic;
using System.Linq;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022.Solutions;

public class Day11 : AdventOfCodeBaseSolution
{
    [TestCase(Part.Part1, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, TestName = "Part 2 solution")]
    public void Solve(Part part)
    {
        var monkeys = Input
            .ReadAllLines()
            .SplitBy(string.IsNullOrWhiteSpace)
            .Select(Monkey.Parse)
            .ToArray();

        PrintSolution(part,
            () => Solve(monkeys, 20, w => (int)Math.Floor(w / 3d)),
            () => Solve(monkeys, 10000, w => w % monkeys.Select(m => m.DivisibleBy).Aggregate(1, (x, y) => x * y)));
    }

    [TestCase(ExpectedResult = "10605")]
    public string SolveTestPart1()
    {
        var monkeys =
            "Monkey 0:\n  Starting items: 79, 98\n  Operation: new = old * 19\n  Test: divisible by 23\n    If true: throw to monkey 2\n    If false: throw to monkey 3\n\nMonkey 1:\n  Starting items: 54, 65, 75, 74\n  Operation: new = old + 6\n  Test: divisible by 19\n    If true: throw to monkey 2\n    If false: throw to monkey 0\n\nMonkey 2:\n  Starting items: 79, 60, 97\n  Operation: new = old * old\n  Test: divisible by 13\n    If true: throw to monkey 1\n    If false: throw to monkey 3\n\nMonkey 3:\n  Starting items: 74\n  Operation: new = old + 3\n  Test: divisible by 17\n    If true: throw to monkey 0\n    If false: throw to monkey 1"
                .Split("\n")
                .SplitBy(string.IsNullOrWhiteSpace)
                .Select(Monkey.Parse)
                .ToArray();

        return Solve(monkeys, 20, w => (int)Math.Floor(w / 3d));
    }


    [TestCase(ExpectedResult = "2713310158")]
    public string SolveTestPart2()
    {
        var monkeys =
            "Monkey 0:\n  Starting items: 79, 98\n  Operation: new = old * 19\n  Test: divisible by 23\n    If true: throw to monkey 2\n    If false: throw to monkey 3\n\nMonkey 1:\n  Starting items: 54, 65, 75, 74\n  Operation: new = old + 6\n  Test: divisible by 19\n    If true: throw to monkey 2\n    If false: throw to monkey 0\n\nMonkey 2:\n  Starting items: 79, 60, 97\n  Operation: new = old * old\n  Test: divisible by 13\n    If true: throw to monkey 1\n    If false: throw to monkey 3\n\nMonkey 3:\n  Starting items: 74\n  Operation: new = old + 3\n  Test: divisible by 17\n    If true: throw to monkey 0\n    If false: throw to monkey 1"
                .Split("\n")
                .SplitBy(string.IsNullOrWhiteSpace)
                .Select(Monkey.Parse)
                .ToArray();

        var divisor = monkeys.Select(m => m.DivisibleBy).Aggregate(1, (x, y) => x * y);
        return Solve(monkeys, 10000, w => w % divisor);
    }


    private static string Solve(Monkey[] monkeys, int rounds, Func<long, long> limit)
    {
        for (var round = 0; round < rounds; round++)
        {
            foreach (var monkey in monkeys)
            {
                foreach (var item in monkey.Items)
                {
                    var worryLevel = monkey.Operation(item);
                    worryLevel = limit(worryLevel);

                    var destination = worryLevel % monkey.DivisibleBy == 0
                        ? monkey.TrueDestination
                        : monkey.FalseDestination;

                    monkeys[destination].Items.Add(worryLevel);
                }

                monkey.Inspections += monkey.Items.Count;
                monkey.Items.Clear();
            }

            //TestContext.WriteLine($"=== Round {round} ===");
            //Print(monkeys);
            //TestContext.WriteLine($"=== End round ===");
        }

        var mostActive = monkeys.OrderByDescending(m => m.Inspections).ToArray();

        return (mostActive[0].Inspections * mostActive[1].Inspections).ToString();
    }

    private static void Print(Monkey[] monkeys)
    {
        foreach (var monkey in monkeys)
        {
            TestContext.WriteLine(monkey.ToString());
        }
    }

    public sealed record Monkey(
        string Name,
        List<long> Items,
        Func<long, long> Operation,
        int DivisibleBy,
        int TrueDestination,
        int FalseDestination
    )
    {
        public override string ToString()
            => $"{Name}: {string.Join(',', Items)}";

        public static Monkey Parse(string[] lines)
        {
            var name = lines[0][..^1];
            var items = lines[1]
                .Split(":", StringSplitOptions.RemoveEmptyEntries)[1]
                .Split(",", StringSplitOptions.TrimEntries)
                .Select(long.Parse)
                .ToList();
            var operation = ParseOperation(lines[2]);
            var divisibleBy = int.Parse(lines[3].Split(" ", StringSplitOptions.RemoveEmptyEntries)[3]);
            var trueDestination = int.Parse(lines[4].Split(" ", StringSplitOptions.RemoveEmptyEntries)[5]);
            var falseDestination = int.Parse(lines[5].Split(" ", StringSplitOptions.RemoveEmptyEntries)[5]);
            return new Monkey(name, items, operation, divisibleBy, trueDestination, falseDestination);
        }

        private static Func<long, long> ParseOperation(string line)
        {
            var words = line.Split("=", StringSplitOptions.TrimEntries)[1].Split(" ");
            return words[1] switch
            {
                "+" => words[2] switch
                {
                    "old" => old => old + old,
                    { } => old => old + long.Parse(words[2]),
                },
                "*" => words[2] switch
                {
                    "old" => old => old * old,
                    { } => old => old * long.Parse(words[2]),
                },
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public long Inspections { get; set; }
    }
}