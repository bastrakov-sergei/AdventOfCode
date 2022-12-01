namespace Day16;

public class OperatorPacket : Packet
{
    public PacketType Type { get; }
    public Packet[] Packets { get; }

    public OperatorPacket(int version, PacketType type, Packet[] packets) : base(version)
    {
        Type = type;
        Packets = packets;
    }

    public override long GetValue()
        => Type switch
        {
            PacketType.Sum => Packets.Select(p => p.GetValue()).Sum(),
            PacketType.Product => Packets.Aggregate(1L, (acc, p) => acc * p.GetValue()),
            PacketType.Min => Packets.Select(p => p.GetValue()).Min(),
            PacketType.Max => Packets.Select(p => p.GetValue()).Max(),
            PacketType.Greater => Packets[0].GetValue() > Packets[1].GetValue() ? 1 : 0,
            PacketType.Less => Packets[0].GetValue() < Packets[1].GetValue() ? 1 : 0,
            PacketType.Equal => Packets[0].GetValue() == Packets[1].GetValue() ? 1 : 0,
            _ => throw new ArgumentOutOfRangeException(),
        };

    public override string ToPrettyString()
    {
        return Type switch
        {
            PacketType.Sum => $"({string.Join("+", Packets.Select(p => p.ToPrettyString()))})",
            PacketType.Product => $"({string.Join("*", Packets.Select(p => p.ToPrettyString()))})",
            PacketType.Min => $"min({string.Join(",", Packets.Select(p => p.ToPrettyString()))})",
            PacketType.Max => $"max({string.Join(",", Packets.Select(p => p.ToPrettyString()))})",
            PacketType.Greater => $"({Packets[0].ToPrettyString()} > {Packets[1].ToPrettyString()})",
            PacketType.Less => $"({Packets[0].ToPrettyString()} < {Packets[1].ToPrettyString()})",
            PacketType.Equal => $"({Packets[0].ToPrettyString()} == {Packets[1].ToPrettyString()})",
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}