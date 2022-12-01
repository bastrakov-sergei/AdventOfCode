public sealed class ForwardCommand : MoveCommand
{
    public ForwardCommand(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public override Submarine Move(Submarine submarine)
        => submarine.Move(Value, 0);
}