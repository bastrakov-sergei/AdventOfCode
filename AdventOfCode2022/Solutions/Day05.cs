using System.Collections.Generic;
using System.Linq;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022.Solutions;

public class Day05 : AdventOfCodeBaseSolution
{
    public Day05() : base()
    {
    }

    [TestCase(Part.Part1, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, TestName = "Part 2 solution")]
    public void Solve(Part part)
    {
        var input = Input.ReadAllLines().ToArray();
        var stacks = Enumerable
            .Range(0, (input[0].Length + 1) / 4)
            .Select(_ => new List<char>())
            .ToArray();

        var fillStacks = true;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                fillStacks = false;
                continue;
            }

            if (fillStacks)
            {
                for (var i = 0; i < stacks.Length; i++)
                    if (char.IsLetter(line[i * 4 + 1]))
                        stacks[i].Add(line[i * 4 + 1]);
            }
            else
            {
                var words = line.Split();
                var (count, from, to) = (int.Parse(words[1]), int.Parse(words[3]) - 1, int.Parse(words[5]) - 1);
                var items = stacks[from].Take(count).ToArray();
                stacks[from].RemoveRange(0, count);
                if (part == Part.Part1) items = items.Reverse().ToArray();
                stacks[to].InsertRange(0, items);
            }
        }

        TestContext.WriteLine($"{part.GetDescription()}: {new string(stacks.Select(stack => stack.First()).ToArray())}");
    }
}