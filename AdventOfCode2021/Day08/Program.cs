using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var dictionary = new Dictionary<int, string>
{
    [0] = "abcefg",
    [1] = "cf",
    [2] = "acdeg",
    [3] = "acdfg",
    [4] = "bcdf",
    [5] = "abdfg",
    [6] = "abdefg",
    [7] = "acf",
    [8] = "abcdefg",
    [9] = "abcdfg"
};

var input = File
    .ReadLines("input.txt")
    .ToArray();

Part1();
Part2(); 

void Part1()
{
    var easyNumbers = dictionary
        .GroupBy(pair => pair.Value.Length)
        .Where(group => @group.Count() == 1)
        .Select(group => @group.First())
        .ToArray();

    Console.WriteLine($"Easy numbers: {string.Join(",", easyNumbers.Select(n => n.Key))}");

    var totalEasyNumbersCount = input
        .Sum(line
            => line.Split("|", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .First()
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Count(digitSignal => easyNumbers.Any(number => number.Value.Length == digitSignal.Length)));

    Console.WriteLine($"Total easy numbers count: {totalEasyNumbersCount}");
}

void Part2()
{
    var parsedInput = input
        .Select(line => line.Split("|", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        .Select(pair => new 
        {
            signals = pair[0]
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(s => new string(s.OrderBy(c => c).ToArray()))
                .ToArray(),
            output = pair[1]
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(s => new string(s.OrderBy(c => c).ToArray()))
                .ToArray()
        })
        .ToArray();

    var sum = 0;
    foreach (var arg in parsedInput)
    {
        sum += Decode(arg.signals, arg.output);
    }

    Console.WriteLine(sum);
}

int Decode(string[] signals, string[] o)
{
    Console.WriteLine($"{string.Join(",", signals)}");
    
    Console.WriteLine("one");
    var one = signals.Single(s => s.Length == 2);
    Console.WriteLine("four");
    var four = signals.Single(s => s.Length == 4);
    Console.WriteLine("seven");
    var seven = signals.Single(s => s.Length == 3);
    Console.WriteLine("eight");
    var eight = signals.Single(s => s.Length == 7);
    
    Console.WriteLine("zero");
    var zero = signals.Single(s => s.Length == 6 && four.Intersect(s).Count() == 3 && one.Intersect(s).Count() == 2);
    Console.WriteLine("six");
    var six = signals.Single(s => s.Length == 6 && seven.Intersect(s).Count() == 2);
    Console.WriteLine("nine");
    var nine = signals.Single(s => s.Length == 6 && four.Intersect(s).Count() == 4);
    
    Console.WriteLine("two");
    var two = signals.Single(s => s.Length == 5 && four.Intersect(s).Count() == 2);
    Console.WriteLine("three");
    var three = signals.Single(s => s.Length == 5 && one.Intersect(s).Count() == 2);
    Console.WriteLine("five");
    var five = signals.Single(s => s.Length == 5 && nine.Intersect(s).Count() == 5 && one.Intersect(s).Count() == 1);

    var map = new Dictionary<string, int>()
    {
        [zero] = 0,
        [one] = 1,
        [two] = 2,
        [three] = 3,
        [four] = 4,
        [five] = 5,
        [six] = 6,
        [seven] = 7,
        [eight] = 8,
        [nine] = 9,
    };

    return o
        .Reverse()
        .Select((s, i) => (int)Math.Pow(10, i) * map[s])
        .Sum();
}