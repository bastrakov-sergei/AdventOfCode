using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");

var inputPolymerTemplate = input.First();
var replacements = input
    .Skip(2)
    .Select(line => line.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
    .ToDictionary(parts => parts[0], parts => parts[1][0]);

var elementCounts = inputPolymerTemplate
    .GroupBy(c => c)
    .ToDictionary(g => g.Key, g => (long) g.Count());

var polymerTemplateCounts = new Dictionary<string, long>();
for (var i = 1; i < inputPolymerTemplate.Length; i++)
{
    var template = inputPolymerTemplate.Substring(i - 1, 2);
    polymerTemplateCounts[template] = polymerTemplateCounts.TryGetValue(template, out var count) ? count + 1 : 1;
}

for (var step = 1; step <= 40; step++)
{
    var polymerTemplateCountsCopy = polymerTemplateCounts.ToDictionary(x => x.Key, x => x.Value);
    foreach (var (polymerTemplate, templateCount) in polymerTemplateCountsCopy)
    {
        if (!replacements.TryGetValue(polymerTemplate, out var replacement))
        {
            continue;
        }

        elementCounts[replacement] = elementCounts.TryGetValue(replacement, out var elementCount)
            ? elementCount + templateCount
            : templateCount;
        polymerTemplateCounts[polymerTemplate] -= templateCount;

        var newPairs = new[]
        {
            polymerTemplate[0] + replacement.ToString(),
            replacement.ToString() + polymerTemplate[1]
        };

        foreach (var newPair in newPairs)
        {
            polymerTemplateCounts[newPair] = polymerTemplateCounts.TryGetValue(newPair, out var count)
                ? count + templateCount
                : templateCount;
        }
    }
}

var result = elementCounts.Max(x => x.Value) - elementCounts.Min(x => x.Value);
Console.WriteLine($"{result}");