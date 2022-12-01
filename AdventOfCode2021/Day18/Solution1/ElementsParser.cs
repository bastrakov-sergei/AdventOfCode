using System;
using System.Linq;

namespace Day18.Solution1;

public static class ElementsParser
{
    public static Element[] Parse(this string[] input)
        => input
            .Select(line => Parse(line.AsSpan()))
            .ToArray();

    private static Element Parse(ReadOnlySpan<char> span)
    {
        ParsePairElement(span, out var result);
        return result;
    }

    private static ReadOnlySpan<char> ParsePairElement(ReadOnlySpan<char> span, out Element value)
    {
        span = span[1..];

        span = ParseElement(span, out var leftElement);
        span = ParseElement(span, out var rightElement);

        value = new PairElement(leftElement, rightElement);

        return span;
    }

    private static ReadOnlySpan<char> ParseElement(ReadOnlySpan<char> span, out Element element)
        => (char.IsDigit(span[0])
            ? ParseValue(span, out element)
            : ParsePairElement(span, out element))[1..];

    private static ReadOnlySpan<char> ParseValue(ReadOnlySpan<char> span, out Element value)
    {
        var index = 0;
        while (char.IsDigit(span[index]))
        {
            index++;
        }

        value = new ValueElement(int.Parse(span[..index]));
        return span[index..];
    }
}