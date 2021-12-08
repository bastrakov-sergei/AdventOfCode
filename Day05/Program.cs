using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day05
{
    public readonly struct IntVector
    {
        public readonly int X;
        public readonly int Y;

        public IntVector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"{X},{Y}";
    }

    public readonly struct Line
    {
        public readonly IntVector Begin;
        public readonly IntVector End;

        public Line(IntVector begin, IntVector end)
        {
            Begin = begin;
            End = end;
        }

        public bool IsVertical => Begin.X == End.X;
        public bool IsHorizontal => Begin.Y == End.Y;
        public bool Is45Degrees => End.Y - Begin.Y == End.X - Begin.X || End.Y - Begin.Y == Begin.X - End.X;

        public bool HasPoint(IntVector point)
        {
            return (point.X - Begin.X) * (End.Y - Begin.Y) == (End.X - Begin.X) * (point.Y - Begin.Y) && 
                   Math.Min(Begin.Y, End.Y) <= point.Y && 
                   Math.Max(Begin.Y, End.Y) >= point.Y && 
                   Math.Min(Begin.X, End.X) <= point.X && 
                   Math.Max(Begin.X, End.X) >= point.X;
        }

        public IEnumerable<IntVector> GetPoints()
        {
            var maxX = Math.Max(Begin.X, End.X);
            var maxY = Math.Max(Begin.Y, End.Y);

            for (var y = 0; y <= maxY; y++)
            {
                for (var x = 0; x <= maxX; x++)
                {
                    var point = new IntVector(x, y);
                    if (HasPoint(point))
                    {
                        yield return point;
                    }
                }
            }
        }

        public override string ToString() => $"{Begin} -> {End}";
    }

    public static class Program
    {
        public static async Task Main()
        {
            var input = await File.ReadAllLinesAsync("input.txt");
            var lines = new List<Line>();
            var field = new Dictionary<IntVector, int>();

            var regex = new Regex("(?<x1>\\d+),(?<y1>\\d+) -> (?<x2>\\d+),(?<y2>\\d+)");
            foreach (var inputLine in input)
            {
                var match = regex.Match(inputLine);
                var x1 = int.Parse(match.Groups["x1"].Value);
                var x2 = int.Parse(match.Groups["x2"].Value);
                var y1 = int.Parse(match.Groups["y1"].Value);
                var y2 = int.Parse(match.Groups["y2"].Value);

                lines.Add(new Line(new IntVector(x1, y1), new IntVector(x2, y2)));
            }

            //PrintDiagram(lines
                //.Where(l => l.IsHorizontal || l.IsVertical || l.Is45Degrees));

            foreach (var point in lines
                .Where(l => l.IsHorizontal || l.IsVertical || l.Is45Degrees)
                .SelectMany(l => l.GetPoints()))
            {
                if (!field.TryGetValue(point, out var count))
                {
                    count = 0;
                }

                count++;
                field[point] = count;
            }

            var result = field.Count(i => i.Value >= 2);
            Console.WriteLine($"Count: {result}");
        }

        private static void PrintDiagram(IEnumerable<Line> lines)
        {
            var enumerable = lines as Line[] ?? lines.ToArray();
            var maxX = enumerable.SelectMany(l => new[] {l.Begin, l.End}).Select(p => p.X).Max();
            var maxY = enumerable.SelectMany(l => new[] {l.Begin, l.End}).Select(p => p.Y).Max();

            for (var y = 0; y <= maxY; y++)
            {
                for (var x = 0; x <= maxX; x++)
                {
                    var point = new IntVector(x, y);
                    var count = enumerable.Count(line => line.HasPoint(point));

                    Console.Write($"{count switch {0 => ".", _ => count}}");
                }

                Console.WriteLine();
            }
        }
    }
}