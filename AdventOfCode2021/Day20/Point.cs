﻿namespace Day20;

public record Point(int X, int Y)
{
    public static Point operator +(Point a, Point b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Point operator *(Point a, int b)
        => new(a.X + b, a.Y + b);

    public static Point operator -(Point a, Point b)
        => new(a.X - b.X, a.Y - b.Y);

    public static readonly Point One = new(1, 1);
    public static readonly Point Zero = new(0, 0);
}