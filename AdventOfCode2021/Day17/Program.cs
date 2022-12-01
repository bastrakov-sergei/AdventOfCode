var input = File
    .ReadAllText("input.txt")["target area: ".Length..];

var parts = input.Split(", ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
var xParts = parts[0][2..].Split("..", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(int.Parse).ToArray();
var yParts = parts[1][2..].Split("..", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(int.Parse).ToArray();

var bounds = new Bounds(new Point(xParts[0], yParts[0]), new Point(xParts[1], yParts[1]));

Console.WriteLine($"{bounds}");

var probes = new List<Probe>();

for (var x = 0; x < 600; x++)
{
    for (var y = -600; y < 600; y++)
    {
        var probe = new Probe(Point.Zero, new Point(x, y));

        while (probe.Position.Y > bounds.Bottom &&
               probe.Position.X < bounds.Right &&
               !bounds.Contains(probe.Position)
              )
        {
            probe.Step();
        }

        if (bounds.Contains(probe.Position))
        {
            probes.Add(probe);
        }
    }
}

var highestProbe = probes.MaxBy(probe => probe.PositionsHistory.Select(p => p.Y).Max());
Console.WriteLine($"Part 1:{highestProbe.PositionsHistory.Select(p => p.Y).Max()}");
Console.WriteLine($"Part 2:{probes.Count}");

public class Probe
{
    public Point Position { get; private set; }
    public Point Velocity { get; private set; }

    public Point InitialPosition { get; }
    public Point InitialVelocity { get; }

    public IReadOnlyCollection<Point> PositionsHistory => positionsHistory.AsReadOnly();
    private readonly List<Point> positionsHistory = new List<Point>();


    public Probe(Point position, Point velocity)
    {
        InitialPosition = position;
        InitialVelocity = velocity;
        Position = position;
        Velocity = velocity;
    }

    public void Step()
    {
        positionsHistory.Add(Position);

        Position += Velocity;
        Velocity += new Point(Math.Sign(Velocity.X) * -1, -1);
    }
}

public class Bounds
{
    private readonly Point topLeft;
    private readonly Point bottomRight;

    public int Top => topLeft.Y;
    public int Bottom => bottomRight.Y;
    public int Left => topLeft.X;
    public int Right => bottomRight.X;

    public int Width => bottomRight.X - topLeft.X;
    public int Height => topLeft.Y - bottomRight.Y;

    public Bounds(Point p1, Point p2)
    {
        topLeft = new Point(Math.Min(p1.X, p2.X), Math.Max(p1.Y, p2.Y));
        bottomRight = new Point(Math.Max(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
    }

    public bool Contains(Point p)
        => topLeft.X <= p.X &&
           bottomRight.X >= p.X &&
           topLeft.Y >= p.Y &&
           bottomRight.Y <= p.Y;

    public override string ToString()
        => $"({topLeft}, {bottomRight})";
}

public record Point(int X, int Y)
{
    private static readonly Point[] NeighborOffsets = { new(-1, 0), new(0, 1), new(1, 0), new(0, -1) };

    public static Point operator +(Point a, Point b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Point operator *(Point a, int b)
        => new(a.X * b, a.Y * b);

    public static Point operator %(Point a, Point b)
        => new(a.X % b.X, a.Y % b.Y);

    public IEnumerable<Point> GetNeighbors()
        => NeighborOffsets.Select(offset => offset + this);

    public override string ToString() => $"({X}, {Y})";

    public static readonly Point Zero = new Point(0, 0);
}