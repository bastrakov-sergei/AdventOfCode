using Day18.Solution1;
using Day18.Solution2;

var input = File.ReadAllLines("input.txt");
var elements = input.Parse();

Console.WriteLine(string.Join("\n", elements.Select(e => e.ToString())));
Console.WriteLine(elements.Aggregate((acc, e) => acc + e).ToString());
Console.WriteLine($"Part 1: {elements.Aggregate((acc, e) => acc + e).GetMagnitude()}");

var maxMagnitude = elements
    .SelectMany(_ => elements, (a, b) => new { a, b })
    .Where(t => t.a != t.b)
    .Select(t => t.a + t.b)
    .Select(sum => sum.GetMagnitude())
    .Max();
Console.WriteLine($"Part 2: {maxMagnitude}");


Console.WriteLine("Other solution");
var snailfishNumbers = input.ParseSnailfishNumbers();
Console.WriteLine($"Part 1: {snailfishNumbers.Aggregate((a, b) => a.Add(b)).GetMagnitude()}");

var otherMaxMagnitude = snailfishNumbers
    .SelectMany(_ => snailfishNumbers, (a, b) => new { a, b })
    .Where(t => t.a != t.b)
    .Select(t => t.a.Add(t.b))
    .Select(sum => sum.GetMagnitude())
    .Max();
Console.WriteLine($"Part 2: {otherMaxMagnitude}");