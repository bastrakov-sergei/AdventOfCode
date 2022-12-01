using System;

namespace Day18.Solution1;

public abstract class Element
{
    public abstract int GetMagnitude();

    private static Element Reduce(Element element)
    {
        while (true)
        {
            if (TryExplode(element, out var explodeResult))
            {
                element = explodeResult;
                continue;
            }

            if (TrySplit(element, out var splitResult))
            {
                element = splitResult;
                continue;
            }

            return element;
        }
    }

    private static bool TryExplode(Element element, out Element result)
    {
        if (TryExplode(element, 0, out var explodeResult))
        {
            result = explodeResult.New;
            return true;
        }

        result = element;
        return false;
    }

    private static bool TrySplit(Element element, out Element result)
    {
        switch (element)
        {
            case ValueElement { Value: >= 10 } valueElement:
                result = valueElement.Split();
                return true;

            case PairElement pairElement:
                if (TrySplit(pairElement.Left, out var leftSplitResult))
                {
                    result = new PairElement(leftSplitResult, pairElement.Right);
                    return true;
                }

                if (TrySplit(pairElement.Right, out var rightSplitResult))
                {
                    result = new PairElement(pairElement.Left, rightSplitResult);
                    return true;
                }

                result = new PairElement(pairElement.Left, pairElement.Right);
                return false;
        }

        result = element;
        return false;
    }

    public static Element operator +(Element left, Element right)
        => Reduce(new PairElement(left, right));

    public static Element operator +(int addition, Element element)
        => element switch
        {
            ValueElement valueElement => valueElement + addition,
            PairElement pairElement => new PairElement(addition + pairElement.Left, pairElement.Right),
            _ => throw new ArgumentOutOfRangeException(),
        };

    public static Element operator +(Element element, int addition)
        => element switch
        {
            ValueElement valueElement => valueElement + addition,
            PairElement pairElement => new PairElement(pairElement.Left, pairElement.Right + addition),
            _ => throw new ArgumentOutOfRangeException(),
        };

    private static bool TryExplode(
        Element current,
        int depth,
        out (Element New, int LeftAddition, int RightAddition) result)
    {
        switch (current)
        {
            case ValueElement:
                result = (New: current, 0, 0);
                return false;
            case PairElement pairElement when depth < 4:
            {
                if (TryExplode(pairElement.Left, depth + 1, out var leftExplodeResult))
                {
                    result = (
                        new PairElement(leftExplodeResult.New, leftExplodeResult.RightAddition + pairElement.Right),
                        leftExplodeResult.LeftAddition,
                        0);
                    return true;
                }

                if (TryExplode(pairElement.Right, depth + 1, out var rightExplodeResult))
                {
                    result = (
                        new PairElement(pairElement.Left + rightExplodeResult.LeftAddition, rightExplodeResult.New),
                        0,
                        rightExplodeResult.RightAddition);
                    return true;
                }

                result = (current, 0, 0);
                return false;
            }
            case PairElement { Left: ValueElement leftElement, Right: ValueElement rightElement }:
                result = (new ValueElement(0), leftElement.Value, rightElement.Value);
                return true;
        }

        throw new Exception();
    }
}