namespace Day18.Solution1;

public class PairElement : Element
{
    public Element Left { get; }

    public Element Right { get; }

    public PairElement(Element left, Element right)
    {
        Left = left;
        Right = right;
    }

    public override string ToString()
        => $"[{Left.ToString()}, {Right.ToString()}]";

    public override int GetMagnitude()
        => Left.GetMagnitude() * 3 + Right.GetMagnitude() * 2;
}