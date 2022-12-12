using System;
using System.Linq;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022.Solutions;

public class Day06 : AdventOfCodeBaseSolution
{
    public Day06() : base()
    {
    }

    [TestCase(Part.Part1, 4, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, 14, TestName = "Part 2 solution")]
    public void Solve(Part part, int count)
    {
        var input = Input.ReadAllLines();
        PrintSolution(part, () => Solve(input.First(), count), () => Solve(input.First(), count));
    }

    [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 4, ExpectedResult = "7")]
    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 4, ExpectedResult = "5")]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 4, ExpectedResult = "6")]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 4, ExpectedResult = "10")]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 4, ExpectedResult = "11")]
    [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 14, ExpectedResult = "19")]
    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 14, ExpectedResult = "23")]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 14, ExpectedResult = "23")]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 14, ExpectedResult = "29")]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 14, ExpectedResult = "26")]
    public string SolveTest(string input, int count)
        => Solve(input, count);

    private static string Solve(string input, int count)
    {
        for (var i = count; i <= input.Length; i++)
        {
            var hashset = input[(i - count)..i].ToHashSet();
            if (hashset.Count == count)
            {
                return i.ToString();
            }
        }

        throw new Exception("No solution");
    }
}