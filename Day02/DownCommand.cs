public sealed class DownCommand : MoveCommand
{
    public DownCommand(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public override Submarine Move(Submarine submarine)
        => submarine.Move(0, Value);
}