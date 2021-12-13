var input = File.ReadAllLines("input.txt");

var dots = new List<(int x, int y)>();
var instructions = new List<(bool isHorizontal, int value)>();

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

        var isHorizontal = parts[0] == "y";
        var value = int.Parse(parts[1]);

        instructions.Add((isHorizontal, value));
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

var paper = new Paper(dots.ToArray());
//paper.Print();

foreach (var (isHorizontal, value) in instructions)
{
    paper = paper.Fold(isHorizontal, value);
    //paper.Print();
    Console.WriteLine($"Dots count: {paper.DotsCount}");
}

paper.Print();


public sealed class Paper
{
    private readonly int width;
    private readonly int height;
    private readonly HashSet<(int x, int y)> dots;

    private Paper(int width, int height, IEnumerable<(int x, int y)> dots)
    {
        this.width = width;
        this.height = height;
        this.dots = new HashSet<(int x, int y)>(dots);
    }

    public Paper((int x, int y)[] dots)
    {
        width = dots.Select(dot => dot.x).Max() + 1;
        height = dots.Select(dot => dot.y).Max() + 1;
        this.dots = new HashSet<(int x, int y)>(dots);
    }

    public int DotsCount => dots.Count;

    public Paper Fold(bool isHorizontal, int value)
    {
        var newDots = new List<(int x, int y)>();

        foreach (var dot in dots)
        {
            switch (isHorizontal)
            {
                case true when dot.y < value:
                case false when dot.x < value:
                    newDots.Add(dot);
                    break;
                default:
                    newDots.Add(Fold(dot, isHorizontal, value));
                    break;
            }
        }

        return new Paper(
            isHorizontal
                ? width
                : value,
            isHorizontal
                ? value
                : height,
            newDots.ToArray()
        );
    }

    private static (int x, int y) Fold((int x, int y) dot, bool isHorizontal, int value)
        => isHorizontal
            ? (dot.x, value * 2 - dot.y)
            : (value * 2 - dot.x, dot.y);
    public void Print()
    {
        Console.WriteLine("---------------------");

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Console.Write(dots.Contains((x, y)) ? "#" : ".");
            }

            Console.WriteLine();
        }

        Console.WriteLine("---------------------");
    }
}