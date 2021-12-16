namespace Day16;

public static class Parser
{
    public static Packet Parse(this string input)
    {
        var inputSpan = input
            .ToBinary()
            .AsSpan();
        ReadPacket(inputSpan, out var packet);

        return packet;
    }

    private static ReadOnlySpan<char> ReadPacket(ReadOnlySpan<char> inputSpan, out Packet packet)
    {
        inputSpan = inputSpan.ReadInt(3, out var version);
        inputSpan = inputSpan.ReadEnum<PacketType>(3, out var type);

        switch (type)
        {
            case PacketType.Literal:
                inputSpan = ReadLiteral(inputSpan, out var value);
                packet = new LiteralPacket(version, value);
                break;
            case PacketType.Sum:
            case PacketType.Product:
            case PacketType.Greater:
            case PacketType.Less:
            case PacketType.Max:
            case PacketType.Min:
            case PacketType.Equal:
                inputSpan = inputSpan.ReadInt(1, out var lengthTypeId);
                switch (lengthTypeId)
                {
                    case 0:
                        inputSpan = inputSpan.ReadInt(15, out var length);
                        inputSpan = ReadPackets(inputSpan, out var innerPackets, maxBitRead: length);
                        packet = new OperatorPacket(version, type, innerPackets);
                        break;
                    case 1:
                        inputSpan = inputSpan.ReadInt(11, out var packetsCount);
                        inputSpan = ReadPackets(inputSpan, out var packets, packetsCount);
                        packet = new OperatorPacket(version, type, packets.ToArray());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown length type id {lengthTypeId}");
                }

                break;
            default:
                throw new ArgumentOutOfRangeException($"Unknown operation type {type}");
        }

        return inputSpan;
    }

    private static ReadOnlySpan<char> ReadPackets(
        ReadOnlySpan<char> inputSpan,
        out Packet[] packets,
        int maxPacketCount = int.MaxValue,
        int maxBitRead = int.MaxValue)
    {
        var result = new List<Packet>();
        var spanLength = inputSpan.Length;

        while (!inputSpan.IsEmpty &&
               result.Count < maxPacketCount &&
               spanLength - inputSpan.Length < maxBitRead
              )
        {
            inputSpan = ReadPacket(inputSpan, out var packet);
            result.Add(packet);
        }

        packets = result.ToArray();
        return inputSpan;
    }

    private static ReadOnlySpan<char> ReadLiteral(ReadOnlySpan<char> inputSpan, out long value)
    {
        var binaryString = new List<char>();
        while (inputSpan[0] == '1')
        {
            inputSpan = ReadBlock(inputSpan);
        }

        inputSpan = ReadBlock(inputSpan);

        value = binaryString.ConvertToLong();
        return inputSpan;

        ReadOnlySpan<char> ReadBlock(ReadOnlySpan<char> readOnlySpan)
        {
            readOnlySpan = readOnlySpan.Head(5, out var literalSpan);
            var literal = literalSpan[1..];

            foreach (var c in literal)
            {
                binaryString.Add(c);
            }

            return readOnlySpan;
        }
    }
}