using System.ComponentModel;
using System.Text;

namespace AOCCommon;

public enum Part
{
    [Description("Part 1")] Part1 = 1,
    [Description("Part 2")] Part2 = 2,
}

public readonly record struct Vector(int X, int Y)
{
    public static Vector operator +(Vector a, Vector b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Vector operator *(Vector a, int b)
        => new(a.X + b, a.Y + b);

    public static Vector operator -(Vector a, Vector b)
        => new(a.X - b.X, a.Y - b.Y);

    public double SqrLength
        => X * X + Y * Y;

    public double Length
        => Math.Sqrt(SqrLength);

    public Vector Direction
        => X == 0 && Y == 0
            ? this
            : new Vector(Math.Sign(X), Math.Sign(Y));

    public override string ToString()
        => $"Vector {{ X = {X}, Y = {Y} }}";

    public static readonly Vector One = new(1, 1);
    public static readonly Vector Zero = new(0, 0);
    public static readonly Vector Up = new(0, 1);
    public static readonly Vector Down = new(0, -1);
    public static readonly Vector Right = new(1, 0);
    public static readonly Vector Left = new(-1, 0);
}