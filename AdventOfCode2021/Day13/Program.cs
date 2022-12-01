var input = File.ReadAllLines("input.txt");

var dots = new List<(int x, int y)>();
var folds = new List<(int x, int y)>();

foreach (var line in input)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        continue;
    }

    if (line.StartsWith("fold"))
    {
        var instruction = line["fold along ".Length..];
        var parts = instruction
            .Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var value = int.Parse(parts[1]);
        folds.Add((parts[0] == "y" ? 0 : value, parts[0] == "x" ? 0 : value));
    }
    else
    {
        var dot = line
            .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
        dots.Add((dot[0], dot[1]));
    }
}

var paper = folds.Aggregate(new Paper(dots.ToArray()), (current, fold) => current.Fold(fold));
paper.Print();


public sealed class Paper
{
    private readonly HashSet<(int x, int y)> dots;

    public Paper(IEnumerable<(int, int)> dots)
    {
        this.dots = new HashSet<(int x, int y)>(dots);
        Console.WriteLine($"Dots count: {this.dots.Count}");
    }

    public Paper Fold((int x, int y) fold)
        => new(dots.Select(dot =>
                fold.x > 0 && dot.x < fold.x ||
                fold.y > 0 && dot.y < fold.y
                    ? dot
                    : (Math.Abs(fold.x * 2 - dot.x), Math.Abs(fold.y * 2 - dot.y)))
        );

    public void Print()
    {
        Console.WriteLine("---------------------");

        var width = dots.Select(dot => dot.x).Max() + 1;
        var height = dots.Select(dot => dot.y).Max() + 1;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Console.Write(dots.Contains((x, y)) ? "█" : " ");
            }

            Console.WriteLine();
        }

        Console.WriteLine("---------------------");
    }
}