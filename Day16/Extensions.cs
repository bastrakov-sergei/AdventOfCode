using System.Text;

namespace Day16;

public static class Extensions
{
    public static long ConvertToLong(this IEnumerable<char> binaryString)
    {
        var binaryStringArray = binaryString.ToArray();
        return binaryStringArray
            .Select((t, i) => (long) Math.Pow(2, i) * (binaryStringArray[binaryStringArray.Length - i - 1] - '0'))
            .Sum();
    }

    public static ReadOnlySpan<char> ReadInt(this ReadOnlySpan<char> inputSpan, int bitsCount, out int result)
    {
        var span = inputSpan.Head(bitsCount, out var subSpan);
        result = subSpan.ConvertToInt();
        return span;
    }

    private static int ConvertToInt(this ReadOnlySpan<char> binaryString)
    {
        var result = 0;
        for (var i = 0; i < binaryString.Length; i++)
        {
            result += (int) Math.Pow(2, i) * (binaryString[binaryString.Length - i - 1] - '0');
        }

        return result;
    }

    public static ReadOnlySpan<T> Head<T>(this ReadOnlySpan<T> input, int offset, out ReadOnlySpan<T> result)
    {
        result = input[..offset];
        return input[offset..];
    }

    private static readonly Dictionary<char, string> HexToBinaryMap = new()
    {
        { '0', "0000" },
        { '1', "0001" },
        { '2', "0010" },
        { '3', "0011" },
        { '4', "0100" },
        { '5', "0101" },
        { '6', "0110" },
        { '7', "0111" },
        { '8', "1000" },
        { '9', "1001" },
        { 'A', "1010" },
        { 'B', "1011" },
        { 'C', "1100" },
        { 'D', "1101" },
        { 'E', "1110" },
        { 'F', "1111" },
    };

    public static string ToBinary(this string input)
        => input
            .Aggregate(new StringBuilder(),
                (acc, c) => acc.Append(HexToBinaryMap.TryGetValue(c, out var value) ? value : c.ToString()))
            .ToString();
}