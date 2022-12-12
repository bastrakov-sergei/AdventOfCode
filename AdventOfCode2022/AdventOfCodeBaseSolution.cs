using System;
using System.IO;
using System.Threading.Tasks;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022
{
    [TestFixture]
    public abstract class AdventOfCodeBaseSolution
    {
        private readonly int contestDay;

        protected StreamReader Input { get; private set; } = default!;

        protected AdventOfCodeBaseSolution()
        {
            this.contestDay = GetContestDayFromClassName();
        }

        [SetUp]
        protected virtual async Task Setup()
        {
            Input = await InputCache.GetInputAsync(contestDay);
        }

        protected static void PrintSolution<T>(Part part, Func<T> part1Solution, Func<T> part2Solution)
        {
            TestContext.WriteLine($@"{part.GetDescription()}: {part switch
                {
                    Part.Part1 => part1Solution(),
                    Part.Part2 => part2Solution(),
                    _ => throw new ArgumentOutOfRangeException(nameof(part), part, null),
                }
            }");
        }

        private int GetContestDayFromClassName()
        {
            var type = GetType();
            var dayString = type.Name[^2..];
            return int.Parse(dayString);
        }
    }
}