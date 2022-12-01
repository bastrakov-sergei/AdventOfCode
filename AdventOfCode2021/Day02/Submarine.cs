public sealed class Submarine
{
    public Submarine(int position, int depth, int aim)
    {
        Position = position;
        Depth = depth;
        Aim = aim;
    }

    public int Position { get; }
    public int Depth { get; }
    public int Aim { get; }

    public Submarine Move(int horizontal, int vertical)
        => new(Position + horizontal, Depth + Aim * horizontal, Aim + vertical);
}