using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Day06
{
    public static class Program
    {
        public static async Task Main()
        {
            var input = await File.ReadAllTextAsync("input.txt");
            var fishes = input
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => new BigInteger(x.Count()));
            for (var days = 0; days < 256; days++)
            {
                fishes = new Dictionary<int, BigInteger>
                {
                    [8] = fishes.GetValueOrDefault(0),
                    [7] = fishes.GetValueOrDefault(8),
                    [6] = fishes.GetValueOrDefault(0) + fishes.GetValueOrDefault(7),
                    [5] = fishes.GetValueOrDefault(6),
                    [4] = fishes.GetValueOrDefault(5),
                    [3] = fishes.GetValueOrDefault(4),
                    [2] = fishes.GetValueOrDefault(3),
                    [1] = fishes.GetValueOrDefault(2),
                    [0] = fishes.GetValueOrDefault(1),
                };
            }

            var total = fishes.Values.Aggregate(new BigInteger(0), (a, b) => a + b);

            Console.WriteLine($"Total: {total}");
        }
    }
}