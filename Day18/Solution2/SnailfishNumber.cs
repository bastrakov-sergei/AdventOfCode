using System.Collections.Immutable;

namespace Day18.Solution2;

public record SnailfishNumber(ImmutableList<Digit> Digits)
{
    public override string ToString()
        => string.Join(",", Digits.Select(d => d.ToString()));

    public int GetMagnitude()
    {
        var digitsCopy = Digits.ToImmutableList();
        var maxDepth = digitsCopy.Select(d => d.Depth).Max();

        while (maxDepth > 0)
        {
            // ReSharper disable once AccessToModifiedClosure
            var left = digitsCopy.FindIndex(d => d.Depth == maxDepth);
            if (left == -1)
            {
                maxDepth--;
                continue;
            }

            var right = left + 1;
            var magnitude = digitsCopy[left].Value * 3 + digitsCopy[right].Value * 2;

            digitsCopy = digitsCopy
                .RemoveAt(left)
                .RemoveAt(left)
                .Insert(left, new Digit(magnitude, maxDepth - 1));
        }

        return digitsCopy.First().Value;
    }

    public SnailfishNumber Reduce()
    {
        var current = this;
        while (true)
        {
            var next = current.Explode();
            if (next != current)
            {
                current = next;
                continue;
            }

            next = current.Split();
            if (next != current)
            {
                current = next;
                continue;
            }

            return current;
        }
    }

    public SnailfishNumber Explode()
    {
        for (var i = 1; i < Digits.Count; i++)
        {
            if (Digits[i - 1].Depth != Digits[i].Depth)
            {
                continue;
            }

            if (Digits[i].Depth > 4)
            {
                var left = Digits[i - 1];
                var right = Digits[i];

                var before = Digits.Take(i - 1).ToImmutableList();
                var after = Digits.Skip(i + 1).ToImmutableList();

                if (before.Any())
                {
                    var (value, depth) = before.Last();
                    before = before
                        .RemoveAt(before.Count - 1)
                        .Add(new Digit(value + left.Value, depth));
                }

                if (after.Any())
                {
                    var (value, depth) = after.First();
                    after = after
                        .RemoveAt(0)
                        .Insert(0, new Digit(value + right.Value, depth));
                }

                return new SnailfishNumber(before.Add(new Digit(0, left.Depth - 1)).AddRange(after));
            }
        }

        return this;
    }

    public SnailfishNumber Split()
    {
        var index = Digits.FindIndex(d => d.Value > 9);
        if (index == -1)
        {
            return this;
        }

        var (value, depth) = Digits[index];

        var before = Digits.Take(index).ToImmutableList();
        var after = Digits.Skip(index + 1).ToImmutableList();

        return new SnailfishNumber(before
            .Add(new Digit((int) Math.Floor(value / 2D), depth + 1))
            .Add(new Digit((int) Math.Ceiling(value / 2D), depth + 1))
            .AddRange(after)
        );
    }
}