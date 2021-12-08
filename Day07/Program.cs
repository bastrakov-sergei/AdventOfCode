using System;
using System.IO;
using System.Linq;

var crabs = File
    .ReadAllText("input.txt")
    .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .OrderBy(c => c)
    .ToArray();

Part1();
Part2();

void Part1()
{
    var median = crabs.Length % 2 == 0
        ? crabs[crabs.Length / 2]
        : crabs[(crabs.Length - 1) / 2 - 1];

    var fuel = crabs
        .Select(c => Math.Abs(c - median))
        .Sum();

    Console.WriteLine(fuel);
}

void Part2()
{
    var min = crabs.First();
    var max = crabs.Last();

    var fuel = Enumerable
        .Range(min, max - min + 1)
        .Select(point => crabs
            .Select(c =>
            {
                var n = Math.Abs(c - point);
                return n * (n + 1) / 2;
            })
            .Sum())
        .Min();

    Console.WriteLine(fuel);
}