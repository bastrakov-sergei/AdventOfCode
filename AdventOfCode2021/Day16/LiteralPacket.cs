namespace Day16;

public class LiteralPacket : Packet
{
    public long Value { get; }

    public LiteralPacket(int version, long value) : base(version)
        => Value = value;

    public override long GetValue() => Value;

    public override string ToPrettyString()
        => Value.ToString();
}