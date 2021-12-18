using System.Collections.Immutable;

namespace Day18.Solution2;

public static class DigitExtensions
{
    public static SnailfishNumber Add(this SnailfishNumber left, SnailfishNumber right)
        => new SnailfishNumber(left
                .Digits
                .AddRange(right.Digits)
                .Select(d => new Digit(d.Value, d.Depth + 1))
                .ToImmutableList())
            .Reduce();


    public static SnailfishNumber[] ParseSnailfishNumbers(this string[] input)
    {
        var numbers = new List<SnailfishNumber>();
        foreach (var line in input)
        {
            var digits = new List<Digit>();
            var span = line.AsSpan();
            var depth = 0;
            while (!span.IsEmpty)
            {
                switch (span[0])
                {
                    case '[':
                        depth++;
                        span = span[1..];
                        continue;
                    case ']':
                        depth--;
                        span = span[1..];
                        continue;
                    case >= '0' and <= '9':
                        var i = 0;
                        while (char.IsDigit(span[i]))
                        {
                            i++;
                        }

                        digits.Add(new Digit(int.Parse(span[..i]), depth));
                        span = span[i..];
                        continue;
                }

                span = span[1..];
            }

            numbers.Add(new SnailfishNumber(digits.ToImmutableList()));
        }

        return numbers.ToArray();
    }
}