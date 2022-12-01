using System.Collections.Immutable;

var input = File.ReadAllLines("input.txt");

var height = 0;
var width = 0;
var eastFacing = ImmutableHashSet<Vector>.Empty;
var southFacing = ImmutableHashSet<Vector>.Empty;

foreach (var line in input)
{
    width = line.Length;
    for (var i = 0; i < line.Length; i++)
    {
        switch (line[i])
        {
            case '>':
                eastFacing = eastFacing.Add(new Vector(i, height));
                break;
            case 'v':
                southFacing = southFacing.Add(new Vector(i, height));
                break;
            default:
                continue;
        }
    }

    height++;
}

var state = new State(eastFacing, southFacing, width, height, 0);

bool hasMoves;
do
{
    (state, hasMoves) = state.DoStep();
} while (hasMoves);

Console.WriteLine($"Part 1: {state.Step}");

public record State(ImmutableHashSet<Vector> EastFacing, ImmutableHashSet<Vector> SouthFacing, int Width, int Height, int Step)
{
    private readonly Vector eastMoveVector = new(1, 0);
    private readonly Vector southMoveVector = new(0, 1);

    public (State next, bool hasMoves) DoStep()
    {
        bool hasRightMoves;
        bool hasBottomMoves;
        State next;

        (next, hasRightMoves) = MoveRight();
        (next, hasBottomMoves) = next.MoveBottom();

        return (next with
        {
            Step = Step + 1,
        }, hasRightMoves || hasBottomMoves);
    }

    private (State state, bool hasMoves) MoveRight()
    {
        var canMove = EastFacing
            .Where(p => CanMove(Sum(p, eastMoveVector)))
            .ToImmutableArray();

        return (this with
        {
            EastFacing = canMove
                .Aggregate(EastFacing, (acc, point) => acc
                    .Remove(point)
                    .Add(Sum(point, eastMoveVector))),
        }, canMove.Length > 0);
    }

    private (State state, bool hasMoves) MoveBottom()
    {
        var canMove = SouthFacing
            .Where(p => CanMove(Sum(p, southMoveVector)))
            .ToImmutableArray();

        return (this with
        {
            SouthFacing = canMove
                .Aggregate(SouthFacing, (acc, point) => acc
                    .Remove(point)
                    .Add(Sum(point, southMoveVector)))
        }, canMove.Length > 0);
    }

    private Vector Sum(Vector a, Vector b)
        => new((a.X + b.X) % Width, (a.Y + b.Y) % Height);

    public bool CanMove(Vector target)
        => !EastFacing.Contains(target) &&
           !SouthFacing.Contains(target);

    public void Print()
    {
        Console.WriteLine();
        Console.WriteLine($"After {Step} step:");
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var point = new Vector(x, y);
                if (EastFacing.Contains(point))
                {
                    Console.Write(">");
                }
                else if (SouthFacing.Contains(point))
                {
                    Console.Write("v");
                }
                else
                {
                    Console.Write(".");
                }
            }

            Console.WriteLine();
        }
    }
}


public readonly record struct Vector(int X, int Y)
{
    public static Vector operator +(Vector a, Vector b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Vector operator *(Vector a, int b)
        => new(a.X + b, a.Y + b);

    public static Vector operator -(Vector a, Vector b)
        => new(a.X - b.X, a.Y - b.Y);

    public static readonly Vector One = new(1, 1);
    public static readonly Vector Zero = new(0, 0);
}