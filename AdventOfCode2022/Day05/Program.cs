var solution1 = await Solve(true);
var solution2 = await Solve(false);
Console.WriteLine($"Solution 1: {solution1}");
Console.WriteLine($"Solution 2: {solution2}");

async Task<string> Solve(bool isPart1)
{
    var input = await File.ReadAllLinesAsync("input.txt");
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
            if (isPart1) items = items.Reverse().ToArray();
            stacks[to].InsertRange(0, items);
        }
    }

    return new string(stacks.Select(stack => stack.First()).ToArray());
}