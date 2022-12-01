namespace Day18.Solution1;

public class ValueElement : Element
{
    public int Value { get; }

    public ValueElement(int value)
    {
        Value = value;
    }

    public static ValueElement operator +(ValueElement element, int addition)
        => new(element.Value + addition);

    public override string ToString()
        => Value.ToString();

    public override int GetMagnitude()
        => Value;

    public PairElement Split()
        => new(new ValueElement((int) Math.Floor(Value / 2D)), new ValueElement((int) Math.Ceiling(Value / 2D)));
}