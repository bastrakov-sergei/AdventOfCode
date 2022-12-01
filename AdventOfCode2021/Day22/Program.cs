using System.Text.RegularExpressions;

var input = File
    .ReadAllLines("input.txt")
    .Select(Parse)
    .ToArray();

Part1(input);
Part2(input);

void Part1((Cuboid cuboid, State state)[] cuboids)
{
    var reactorCore = new State[101, 101, 101];

    var xSize = reactorCore.GetLength(0);
    var ySize = reactorCore.GetLength(1);
    var zSize = reactorCore.GetLength(2);

    foreach (var (cuboid, state) in cuboids)
    {
        for (var x = 0; x < xSize; x++)
        {
            for (var y = 0; y < ySize; y++)
            {
                for (var z = 0; z < zSize; z++)
                {
                    if (cuboid.Contains(new Point(x - xSize / 2, y - ySize / 2, z - zSize / 2)))
                    {
                        reactorCore[x, y, z] = state;
                    }
                }
            }
        }
    }


    Console.WriteLine($"Part 1: {CalculateCubesOn()}");

    int CalculateCubesOn()
    {
        var cubesOn = 0;

        for (var x = 0; x < xSize; x++)
        {
            for (var y = 0; y < ySize; y++)
            {
                for (var z = 0; z < zSize; z++)
                {
                    if (reactorCore[x, y, z] == State.On)
                    {
                        cubesOn++;
                    }
                }
            }
        }

        return cubesOn;
    }
}

void Part2((Cuboid cuboid, State state)[] cuboids)
{
    var result = new List<(Cuboid cuboid, State state)>();

    foreach (var (cuboid, state) in cuboids)
    {
        var cubesToAdd = new List<(Cuboid, State)>();
        if (state == State.On)
        {
            cubesToAdd.Add((cuboid, state));
        }

        foreach (var (otherCube, otherState) in result)
        {
            var intersection = Cuboid.GetIntersection(cuboid, otherCube);
            if (intersection != null)
            {
                cubesToAdd.Add((intersection.Value, otherState switch
                {
                    State.Off => State.On, State.On => State.Off,
                }));
            }
        }

        result.AddRange(cubesToAdd);
    }

    Console.WriteLine(
        $"Part 2: {result.Aggregate(0L, (acc, x) => acc + x.cuboid.Size * x.state switch { State.Off => -1, State.On => 1 })}");
}

(Cuboid cuboid, State state) Parse(string line)
{
    var regex = new Regex(
        @"(?<state>on|off)\sx=(?<x1>-?\d+)..(?<x2>-?\d+),y=(?<y1>-?\d+)..(?<y2>-?\d+),z=(?<z1>-?\d+)..(?<z2>-?\d+)",
        RegexOptions.Compiled);
    var match = regex.Match(line);

    var state = match.Groups["state"].Value switch
    {
        "on" => State.On, "off" => State.Off, _ => throw new ArgumentOutOfRangeException()
    };
    var x1 = int.Parse(match.Groups["x1"].Value);
    var x2 = int.Parse(match.Groups["x2"].Value);
    var y1 = int.Parse(match.Groups["y1"].Value);
    var y2 = int.Parse(match.Groups["y2"].Value);
    var z1 = int.Parse(match.Groups["z1"].Value);
    var z2 = int.Parse(match.Groups["z2"].Value);

    var min = new Point(Math.Min(x1, x2), Math.Min(y1, y2), Math.Min(z1, z2));
    var max = new Point(Math.Max(x1, x2), Math.Max(y1, y2), Math.Max(z1, z2));

    return (new Cuboid(min, max), state);
}

public enum State : byte
{
    Off = 0,
    On = 1,
}

public readonly record struct Point(int X, int Y, int Z);

public readonly record struct Cuboid
{
    public Cuboid(Point min, Point max)
    {
        if (min.X > max.X) throw new Exception();
        if (min.Y > max.Y) throw new Exception();
        if (min.Z > max.Z) throw new Exception();

        Min = min;
        Max = max;
    }

    public long Size => (Max.X - Min.X + 1L) * (Max.Y - Min.Y + 1L) * (Max.Z - Min.Z + 1L);
    public Point Min { get; }
    public Point Max { get; }

    public bool Contains(Point point)
        => Min.X <= point.X && point.X <= Max.X &&
           Min.Y <= point.Y && point.Y <= Max.Y &&
           Min.Z <= point.Z && point.Z <= Max.Z;

    public bool Intersect(Cuboid other)
        => Min.X < other.Max.X && other.Min.X < Max.X &&
           Min.Y < other.Max.Y && other.Min.Y < Max.Y &&
           Min.Z < other.Max.Z && other.Min.Z < Max.Z;

    public static Cuboid? GetIntersection(Cuboid a, Cuboid b)
    {
        if (!a.Intersect(b))
        {
            return null;
        }

        return new Cuboid(
            new Point(
                Math.Max(a.Min.X, b.Min.X),
                Math.Max(a.Min.Y, b.Min.Y),
                Math.Max(a.Min.Z, b.Min.Z)),
            new Point(
                Math.Min(a.Max.X, b.Max.X),
                Math.Min(a.Max.Y, b.Max.Y),
                Math.Min(a.Max.Z, b.Max.Z))
        );
    }
}