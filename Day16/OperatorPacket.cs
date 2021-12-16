namespace Day16;

public class OperatorPacket : Packet
{
    public OperatorPacket(int version, int type, Packet[] packets) : base(version)
    {
        Type = type;
        Packets = packets;
    }

    public int Type { get; }
    public Packet[] Packets { get; }

    public override long GetValue()
        => Type switch
        {
            0 => Packets.Select(p => p.GetValue()).Sum(),
            1 => Packets.Aggregate(1L, (acc, p) => acc * p.GetValue()),
            2 => Packets.Select(p => p.GetValue()).Min(),
            3 => Packets.Select(p => p.GetValue()).Max(),
            5 => Packets[0].GetValue() > Packets[1].GetValue() ? 1 : 0,
            6 => Packets[0].GetValue() < Packets[1].GetValue() ? 1 : 0,
            7 => Packets[0].GetValue() == Packets[1].GetValue() ? 1 : 0,
            _ => throw new ArgumentOutOfRangeException(),
        };
}