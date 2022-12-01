public sealed class UpCommand : MoveCommand
{
    public UpCommand(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public override Submarine Move(Submarine submarine)
        => submarine.Move(0, -Value);
}