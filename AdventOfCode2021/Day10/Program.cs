var inputLines = File.ReadAllLines("input.txt");

var bracesMap = new Dictionary<char, char>
{
    ['('] = ')',
    ['['] = ']',
    ['{'] = '}',
    ['<'] = '>',
};
var scoreMap = new Dictionary<char, int>
{
    [')'] = 3,
    [']'] = 57,
    ['}'] = 1197,
    ['>'] = 25137,
};
var closeScoreMap = new Dictionary<char, int>
{
    ['('] = 1,
    ['['] = 2,
    ['{'] = 3,
    ['<'] = 4,
};

Part1();
Part2();

void Part1()
{
    var score = 0L;
    foreach (var inputLine in inputLines)
    {
        var stack = new Stack<char>();
        foreach (var c in inputLine)
        {
            if (bracesMap.ContainsKey(c))
            {
                stack.Push(c);
                continue;
            }

            if (!bracesMap.ContainsValue(c))
            {
                continue;
            }

            if (bracesMap[stack.Peek()] == c)
            {
                stack.Pop();
            }
            else
            {
                score += scoreMap[c];
                break;
            }
        }
    }

    Console.WriteLine($"Part 1: {score}");
}

void Part2()
{
    var scores = inputLines
        .Select(inputLine =>
        {
            var stack = new Stack<char>();
            foreach (var c in inputLine)
            {
                if (bracesMap.ContainsKey(c))
                {
                    stack.Push(c);
                    continue;
                }

                if (!bracesMap.ContainsValue(c))
                {
                    continue;
                }

                if (bracesMap[stack.Peek()] == c)
                {
                    stack.Pop();
                }
                else
                {
                    return null;
                }
            }

            return stack;
        })
        .Where(stack => stack != null)
        .Select(stack =>
        {
            var score = 0L;
            while (stack!.TryPop(out var c))
            {
                score *= 5;
                score += closeScoreMap[c];
            }

            return score;
        })
        .OrderBy(x => x)
        .ToArray();

    Console.WriteLine($"Part 2: {scores[(scores.Length - 1) / 2]}");
}