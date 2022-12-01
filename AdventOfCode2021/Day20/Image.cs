using System;
using System.Collections.Generic;
using System.Linq;

namespace Day20;

public class Image
{
    private readonly Range range;
    private readonly int[] enhanceMap;
    private readonly int backgroundPixel;
    private readonly HashSet<Point> pixels;

    private static readonly Point[] NeighborOffsets =
    {
        new(-1, -1), new(0, -1), new(1, -1),
        new(-1, 0), new(0, 0), new(1, 0),
        new(-1, 1), new(0, 1), new(1, 1),
    };

    public IReadOnlyCollection<Point> Pixels => pixels.Select(p => p - range.TopLeft).ToArray();
    public int Witdh => range.Width;
    public int Height => range.Height;

    public Image(Range range, HashSet<Point> pixels, int[] enhanceMap, int backgroundPixel)
    {
        this.range = range;
        this.enhanceMap = enhanceMap;
        this.backgroundPixel = backgroundPixel;
        this.pixels = pixels;
    }

    public Image Enhance()
    {
        var swapBlackWhite = enhanceMap[0] == 1;
        var newMap = new HashSet<Point>();
        var newRange = range.Enlarge(1);

        foreach (var newPoint in newRange)
        {
            var hash = 0;
            for (var index = 0; index < NeighborOffsets.Length; index++)
            {
                int value;
                if (pixels.Contains(newPoint + NeighborOffsets[index]))
                    value = 1;
                else if (range.Contains(newPoint + NeighborOffsets[index]))
                    value = 0;
                else
                {
                    value = backgroundPixel;
                }

                hash += (int) Math.Pow(2, NeighborOffsets.Length - index - 1) * value;
            }

            var newColor = enhanceMap[hash];
            if (newColor == 1)
            {
                newMap.Add(newPoint);
            }
        }

        return new Image(newRange, newMap, enhanceMap,
            !swapBlackWhite
                ? backgroundPixel
                : backgroundPixel == 0
                    ? 1
                    : 0);
    }

    public void Print()
    {
        Console.WriteLine("-----------");

        foreach (var point in range)
        {
            Console.Write(pixels.Contains(point) ? "#" : ".");

            if (point.X == range.Right - 1)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine("-----------");
    }
}