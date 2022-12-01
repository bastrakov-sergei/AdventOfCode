using System;

public readonly struct Vector3
{
    public readonly int X;
    public readonly int Y;
    public readonly int Z;

    public Vector3(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString()
        => $"(X={X},Y={Y},Z={Z})";

    public static Vector3 operator +(Vector3 a, Vector3 b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3 operator -(Vector3 a)
        => new(-a.X, -a.Y, -a.Z);

    public static Vector3 operator -(Vector3 a, Vector3 b)
        => a + -b;

    public static readonly Vector3 Zero = new(0, 0, 0);

    public static readonly Func<Vector3, Vector3>[] Rotations =
    {
        v => new Vector3(v.X, v.Y, v.Z),
        v => new Vector3(v.X, -v.Z, v.Y),
        v => new Vector3(v.X, -v.Y, -v.Z),
        v => new Vector3(v.X, v.Z, -v.Y),
        v => new Vector3(-v.X, -v.Y, v.Z),
        v => new Vector3(-v.X, -v.Z, -v.Y),
        v => new Vector3(-v.X, v.Y, -v.Z),
        v => new Vector3(-v.X, v.Z, v.Y),
        v => new Vector3(v.Y, v.Z, v.X),
        v => new Vector3(v.Y, -v.X, v.Z),
        v => new Vector3(v.Y, -v.Z, -v.X),
        v => new Vector3(v.Y, v.X, -v.Z),
        v => new Vector3(-v.Y, -v.Z, v.X),
        v => new Vector3(-v.Y, -v.X, -v.Z),
        v => new Vector3(-v.Y, v.Z, -v.X),
        v => new Vector3(-v.Y, v.X, v.Z),
        v => new Vector3(v.Z, v.X, v.Y),
        v => new Vector3(v.Z, -v.Y, v.X),
        v => new Vector3(v.Z, -v.X, -v.Y),
        v => new Vector3(v.Z, v.Y, -v.X),
        v => new Vector3(-v.Z, -v.X, v.Y),
        v => new Vector3(-v.Z, -v.Y, -v.X),
        v => new Vector3(-v.Z, v.X, -v.Y),
        v => new Vector3(-v.Z, v.Y, v.X),
    };
}