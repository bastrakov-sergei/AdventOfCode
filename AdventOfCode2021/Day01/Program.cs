// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day01
{
    public static class Program
    {
        public static async Task Main()
        {
            var input = await File.ReadAllTextAsync("input1.txt");
            var numbers = input
                .Split(new []{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToArray();

            Task1(numbers);
            Task2(numbers);
        }

        private static void Task1(int[] numbers)
        {
            var count = 0;

            for (var i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > numbers[i - 1])
                {
                    count++;
                }
            }

            Console.WriteLine(count);
        }

        private static void Task2(int[] numbers)
        {
            var count = 0;

            for (var i = 3; i < numbers.Length; i++)
            {
                var leftWindowSum = numbers[i - 1] + numbers[i - 2] + numbers[i - 3];
                var rightWindowSum = numbers[i] + numbers[i - 1] + numbers[i - 2];
                
                if (rightWindowSum > leftWindowSum)
                {
                    count++;
                }
            }

            Console.WriteLine(count);
        }
    }
}