namespace Day18.Solution2;

public record Digit(int Value, int Depth)
{
    public override string ToString()
        => $"({Value}-{Depth})";
}