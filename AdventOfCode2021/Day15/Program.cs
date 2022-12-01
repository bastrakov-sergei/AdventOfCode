using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var riskMap = File
    .ReadAllLines("input.txt")
    .SelectMany((line, y) => line.Select((c, x) => (x, y, v: c - '0')))
    .ToDictionary(p => new Point(p.x, p.y), p => p.v);

var start = new Point(0, 0);
var goal = new Point(riskMap.Keys.Max(p => p.X), riskMap.Keys.Max(p => p.Y));

var part1Result = riskMap.FindPathCost(start, goal);

var newRiskMap = riskMap.Increase(5);
var newGoal = new Point(newRiskMap.Keys.Max(p => p.X), newRiskMap.Keys.Max(p => p.Y));
var part2Result = newRiskMap.FindPathCost(start, newGoal);

Console.WriteLine($"Part1: {part1Result}");
Console.WriteLine($"Part2: {part2Result}");


internal static class Extensions
{
    public static Dictionary<Point, int> Increase(this Dictionary<Point, int> riskMap, int factor)
    {
        var newMap = new Dictionary<Point, int>();
        var size = new Point(riskMap.Keys.Max(p => p.X) + 1, riskMap.Keys.Max(p => p.Y) + 1);
        var newSize = new Point(riskMap.Keys.Max(p => p.X) + 1, riskMap.Keys.Max(p => p.Y) + 1) * factor;

        for (var x = 0; x < newSize.X; x++)
        {
            for (var y = 0; y < newSize.Y; y++)
            {
                var point = new Point(x, y);

                var sourcePoint = point % size;
                var value = riskMap[sourcePoint];

                var newValue = (value + x / size.X + y / size.Y - 1) % 9 + 1;
                newMap.Add(point, newValue);
            }
        }

        return newMap;
    }

    public static int FindPathCost(this Dictionary<Point, int> riskMap, Point start, Point goal)
    {
        var queue = new PriorityQueue<Point, int>();
        queue.Enqueue(start, 0);

        var marks = new Dictionary<Point, int>
        {
            [start] = 0,
        };

        while (queue.TryDequeue(out var current, out _))
        {
            foreach (var neighbour in current
                         .GetNeighbors()
                         .Where(riskMap.ContainsKey)
                         .Where(n => !marks.ContainsKey(n))
                    )
            {
                marks[neighbour] = marks[current] + riskMap[neighbour];
                queue.Enqueue(neighbour, marks[neighbour]);
            }
        }

        return marks[goal];
    }
}

internal record Point(int X, int Y)
{
    private static readonly Point[] NeighborOffsets = { new(-1, 0), new(0, 1), new(1, 0), new(0, -1) };

    public static Point operator +(Point a, Point b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Point operator *(Point a, int b)
        => new(a.X * b, a.Y * b);

    public static Point operator %(Point a, Point b)
        => new(a.X % b.X, a.Y % b.Y);

    public IEnumerable<Point> GetNeighbors()
        => NeighborOffsets.Select(offset => offset + this);
}