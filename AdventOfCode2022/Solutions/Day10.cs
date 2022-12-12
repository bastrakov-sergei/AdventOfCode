using System.Collections.Generic;
using System.Linq;
using System.Text;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022;

public class Day10 : AdventOfCodeBaseSolution
{
    public Day10() : base()
    {
    }

    [TestCase(Part.Part1, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, TestName = "Part 2 solution")]
    public void Solve(Part part)
    {
        var input = Input
            .ReadAllLines()
            .ToArray();
        PrintSolution(part, () => SolvePart1(input), () => "\n" + SolvePart2(input));
    }

    [TestCase(ExpectedResult = "13140")]
    public string SolvePart1Test()
    {
        const string input =
            "addx 15\naddx -11\naddx 6\naddx -3\naddx 5\naddx -1\naddx -8\naddx 13\naddx 4\nnoop\naddx -1\naddx 5\naddx -1\naddx 5\naddx -1\naddx 5\naddx -1\naddx 5\naddx -1\naddx -35\naddx 1\naddx 24\naddx -19\naddx 1\naddx 16\naddx -11\nnoop\nnoop\naddx 21\naddx -15\nnoop\nnoop\naddx -3\naddx 9\naddx 1\naddx -3\naddx 8\naddx 1\naddx 5\nnoop\nnoop\nnoop\nnoop\nnoop\naddx -36\nnoop\naddx 1\naddx 7\nnoop\nnoop\nnoop\naddx 2\naddx 6\nnoop\nnoop\nnoop\nnoop\nnoop\naddx 1\nnoop\nnoop\naddx 7\naddx 1\nnoop\naddx -13\naddx 13\naddx 7\nnoop\naddx 1\naddx -33\nnoop\nnoop\nnoop\naddx 2\nnoop\nnoop\nnoop\naddx 8\nnoop\naddx -1\naddx 2\naddx 1\nnoop\naddx 17\naddx -9\naddx 1\naddx 1\naddx -3\naddx 11\nnoop\nnoop\naddx 1\nnoop\naddx 1\nnoop\nnoop\naddx -13\naddx -19\naddx 1\naddx 3\naddx 26\naddx -30\naddx 12\naddx -1\naddx 3\naddx 1\nnoop\nnoop\nnoop\naddx -9\naddx 18\naddx 1\naddx 2\nnoop\nnoop\naddx 9\nnoop\nnoop\nnoop\naddx -1\naddx 2\naddx -37\naddx 1\naddx 3\nnoop\naddx 15\naddx -21\naddx 22\naddx -6\naddx 1\nnoop\naddx 2\naddx 1\nnoop\naddx -10\nnoop\nnoop\naddx 20\naddx 1\naddx 2\naddx 2\naddx -6\naddx -11\nnoop\nnoop\nnoop";

        return SolvePart1(input.Split('\n'));
    }

    [TestCase(ExpectedResult =
        "##..##..##..##..##..##..##..##..##..##..\n###...###...###...###...###...###...###.\n####....####....####....####....####....\n#####.....#####.....#####.....#####.....\n######......######......######......####\n#######.......#######.......#######.....\n")]
    public string SolvePart2Test()
    {
        const string input =
            "addx 15\naddx -11\naddx 6\naddx -3\naddx 5\naddx -1\naddx -8\naddx 13\naddx 4\nnoop\naddx -1\naddx 5\naddx -1\naddx 5\naddx -1\naddx 5\naddx -1\naddx 5\naddx -1\naddx -35\naddx 1\naddx 24\naddx -19\naddx 1\naddx 16\naddx -11\nnoop\nnoop\naddx 21\naddx -15\nnoop\nnoop\naddx -3\naddx 9\naddx 1\naddx -3\naddx 8\naddx 1\naddx 5\nnoop\nnoop\nnoop\nnoop\nnoop\naddx -36\nnoop\naddx 1\naddx 7\nnoop\nnoop\nnoop\naddx 2\naddx 6\nnoop\nnoop\nnoop\nnoop\nnoop\naddx 1\nnoop\nnoop\naddx 7\naddx 1\nnoop\naddx -13\naddx 13\naddx 7\nnoop\naddx 1\naddx -33\nnoop\nnoop\nnoop\naddx 2\nnoop\nnoop\nnoop\naddx 8\nnoop\naddx -1\naddx 2\naddx 1\nnoop\naddx 17\naddx -9\naddx 1\naddx 1\naddx -3\naddx 11\nnoop\nnoop\naddx 1\nnoop\naddx 1\nnoop\nnoop\naddx -13\naddx -19\naddx 1\naddx 3\naddx 26\naddx -30\naddx 12\naddx -1\naddx 3\naddx 1\nnoop\nnoop\nnoop\naddx -9\naddx 18\naddx 1\naddx 2\nnoop\nnoop\naddx 9\nnoop\nnoop\nnoop\naddx -1\naddx 2\naddx -37\naddx 1\naddx 3\nnoop\naddx 15\naddx -21\naddx 22\naddx -6\naddx 1\nnoop\naddx 2\naddx 1\nnoop\naddx -10\nnoop\nnoop\naddx 20\naddx 1\naddx 2\naddx 2\naddx -6\naddx -11\nnoop\nnoop\nnoop";

        return SolvePart2(input.Split('\n'));
    }

    private static string SolvePart1(IEnumerable<string> lines)
    {
        return GetSignalStrengths(Run(lines)).ToString();
    }

    private static string SolvePart2(IEnumerable<string> lines)
    {
        return Print(Run(lines));
    }


    private static Dictionary<int, int> Run(IEnumerable<string> lines)
    {
        var cycle = 1;
        var registerByCycle = new Dictionary<int, int>
        {
            [cycle] = 1,
        };


        foreach (var line in lines)
        {
            var words = line.Split();
            switch (words[0])
            {
                case "noop":
                    cycle++;
                    registerByCycle[cycle] = registerByCycle[cycle - 1];
                    break;
                case "addx":
                    cycle++;
                    registerByCycle[cycle] = registerByCycle[cycle - 1];
                    cycle++;
                    registerByCycle[cycle] = registerByCycle[cycle - 1] + int.Parse(words[1]);
                    break;
            }
        }

        return registerByCycle;
    }

    private static string Print(IReadOnlyDictionary<int, int> registerByCycle)
    {
        var screen = new StringBuilder();
        for (var yi = 0; yi < 6; yi++)
        {
            for (var xi = 0; xi < 40; xi++)
            {
                var crtPos = yi * 40 + xi;
                var spritePos = new[]
                {
                    registerByCycle[crtPos + 1] - 1,
                    registerByCycle[crtPos + 1],
                    registerByCycle[crtPos + 1] + 1,
                };

                var pixel = spritePos.Any(pixelPos => pixelPos == xi) ? '#' : '.';
                screen.Append(pixel);
            }

            screen.Append('\n');
        }

        return screen.ToString();
    }
    
    private static int GetSignalStrengths(IReadOnlyDictionary<int, int> registerByCycle)
        => new[] { 20, 60, 100, 140, 180, 220, }
            .Select(x => x * registerByCycle[x])
            .Sum();
}