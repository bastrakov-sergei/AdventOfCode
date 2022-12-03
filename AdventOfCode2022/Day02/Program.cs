var solution1Score = new[,]
{
    { 4, 1, 7 },
    { 8, 5, 2 },
    { 3, 9, 6 },
};

var solution2Score = new[,]
{
    { 3, 1, 2 },
    { 4, 5, 6 },
    { 8, 9, 7 },
};

var input = (await File.ReadAllLinesAsync("input.txt")).Select(line => (op: line[0] - 'A', me: line[2] - 'X'))
    .ToArray();
var solution1 = input.Select(turn => solution1Score[turn.me, turn.op]).Sum();
var solution2 = input.Select(turn => solution2Score[turn.me, turn.op]).Sum();
Console.WriteLine($"Solution 1: {solution1}");
Console.WriteLine($"Solution 2: {solution2}");