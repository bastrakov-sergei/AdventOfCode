using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");

var width = input[0].Length;
var height = input.Length;
var heightMapValues = input
    .SelectMany(line => line)
    .Select(c => c - '0')
    .ToArray();

var heightMap = new HeightMap(heightMapValues, width, height);
var part1Result = heightMap
    .EnumerateLocalMinimums()
    .Select(min => min + 1)
    .Sum();

Console.WriteLine(part1Result);


var part2Result = heightMap
    .EnumerateBasinSizes()
    .OrderByDescending(x => x)
    .Take(3)
    .Aggregate(1, (i, acc) => i * acc);

Console.WriteLine(part2Result);

public sealed class HeightMap
{
    private readonly int[] heightMap;
    private readonly int width;
    private readonly int height;

    private readonly (int x, int y)[] neighborOffsets =
    {
        (x: 0, y: 1),
        (x: 1, y: 0),
        (x: 0, y: -1),
        (x: -1, y: 0),
    };

    public HeightMap(int[] heightMap, int width, int height)
    {
        this.heightMap = heightMap;
        this.width = width;
        this.height = height;
    }

    public IEnumerable<int> EnumerateBasinSizes()
    {
        var minimums = heightMap
            .Select((v, i) => (value: v, index: i))
            .Where(x
                => GetAdjacentLocationValues(x.index)
                    .All(adjacentLocationValue => x.value < adjacentLocationValue))
            .Select(x => GetCoordinates(x.index))
            .ToArray();

        foreach (var minimum in minimums)
        {
            var queue = new Queue<(int x, int y)>();
            queue.Enqueue(minimum);

            var visited = new HashSet<(int x, int y)>();

            while (queue.TryDequeue(out var point))
            {
                visited.Add(point);
                
                var neighbors = GetAdjacentLocations(point)
                    .Where(neighbor => !visited.Contains(neighbor));
                
                foreach (var neighbor in neighbors)
                {
                    if (heightMap[GetIndex(neighbor)] == 9)
                    {
                        continue;
                    }
                    
                    queue.Enqueue(neighbor);
                }
            }

            yield return visited.Count;
        }
    }

    public IEnumerable<int> EnumerateLocalMinimums() 
        => heightMap
            .Where((value, index)
                => GetAdjacentLocationValues(index)
                    .All(adjacentLocationValue => value < adjacentLocationValue));

    private IEnumerable<(int x, int y)> GetAdjacentLocations((int x, int y) coordinate)
        => neighborOffsets
            .Select(neighborOffset => (x: coordinate.x + neighborOffset.x, y: coordinate.y + neighborOffset.y))
            .Where(neighborCoordinate
                => neighborCoordinate.x >= 0 &&
                   neighborCoordinate.y >= 0 &&
                   neighborCoordinate.x < width &&
                   neighborCoordinate.y < height);

    private IEnumerable<int> GetAdjacentLocationValues(int i) 
        => GetAdjacentLocations(GetCoordinates(i))
            .Select(neighborCoordinate => heightMap[GetIndex(neighborCoordinate)]);

    private (int x, int y) GetCoordinates(int index)
        => (index % width, index / width);

    private int GetIndex((int x, int y) coordinates)
        => coordinates.y * width + coordinates.x;
}