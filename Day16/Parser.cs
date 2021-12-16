namespace Day16;

public static class Parser
{
    public static Packet Parse(this string input)
    {
        var inputSpan = input.ToBinary().AsSpan();
        ReadPacket(inputSpan, out var packet);

        return packet;
    }

    private static ReadOnlySpan<char> ReadPacket(ReadOnlySpan<char> inputSpan, out Packet packet)
    {
        inputSpan = inputSpan.ReadInt(3, out var version);
        inputSpan = inputSpan.ReadInt(3, out var type);

        switch (type)
        {
            case 4:
                inputSpan = ReadLiterals(inputSpan, out var value);
                packet = new LiteralPacket(version, value);
                break;
            case 0:
            case 1:
            case 2:
            case 3:
            case 5:
            case 6:
            case 7:
                inputSpan = inputSpan.ReadInt(1, out var lengthTypeId);
                switch (lengthTypeId)
                {
                    case 0:
                        inputSpan = inputSpan.ReadInt(15, out var length);
                        inputSpan = inputSpan.Head(length, out var innerSpan);

                        var innerPackets = ReadPackets(innerSpan);
                        packet = new OperatorPacket(version, type, innerPackets);
                        break;
                    case 1:
                        inputSpan = inputSpan.ReadInt(11, out var packetsCount);

                        var packets = new List<Packet>();
                        for (var i = 0; i < packetsCount; i++)
                        {
                            inputSpan = ReadPacket(inputSpan, out var innerPacket);
                            packets.Add(innerPacket);
                        }

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

    private static Packet[] ReadPackets(ReadOnlySpan<char> inputSpan)
    {
        var result = new List<Packet>();
        while (!inputSpan.IsEmpty)
        {
            var x = ReadPacket(inputSpan, out var packet);
            inputSpan = x;
            result.Add(packet);
        }

        return result.ToArray();
    }

    private static ReadOnlySpan<char> ReadLiterals(ReadOnlySpan<char> inputSpan, out long value)
    {
        var binaryString = new List<char>();
        while (inputSpan[0] == '1')
        {
            inputSpan = ReadLiteral(inputSpan);
        }

        inputSpan = ReadLiteral(inputSpan);

        value = binaryString.ConvertToLong();
        return inputSpan;

        ReadOnlySpan<char> ReadLiteral(ReadOnlySpan<char> readOnlySpan)
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