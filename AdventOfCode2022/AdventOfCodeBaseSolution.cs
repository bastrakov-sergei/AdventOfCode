using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using AOCCommon;
using Flurl.Http;
using NUnit.Framework;

namespace AdventOfCode2022
{
    [TestFixture]
    public abstract class AdventOfCodeBaseSolution
    {
        private const string LocalCachePath = "../../../inputCache";
        private readonly int contestDay;

        private static int ContestYear => int.TryParse(Environment.GetEnvironmentVariable("ContestYear"), out var year)
            ? year
            : DateTime.Now.Year;

        protected StreamReader Input { get; private set; } = default!;

        protected AdventOfCodeBaseSolution(int contestDay)
        {
            if (contestDay is <= 0 or > 25)
            {
                throw new ArgumentOutOfRangeException(nameof(contestDay));
            }

            this.contestDay = contestDay;
        }

        [SetUp]
        protected virtual async Task Setup()
        {
            Input = await GetInputAsync();
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

        private async Task<StreamReader> GetInputAsync()
        {
            if (TryLoadLocalFile(out var streamReader))
            {
                return streamReader;
            }

            return await LoadRemoteFileAsync();
        }

        private async Task<StreamReader> LoadRemoteFileAsync()
        {
            var session = Environment.GetEnvironmentVariable("AdventOfCodeSession")
                          ?? throw new InvalidOperationException("Session cookie not specified");
            var rawBytes = await BuildRemoteFilePath().WithCookie("session", session).GetBytesAsync();
            var path = BuildLocalFilePath();
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            await File.WriteAllBytesAsync(path, rawBytes);

            if (TryLoadLocalFile(out var reader))
            {
                return reader;
            }

            throw new Exception("Some shit happened");
        }

        private bool TryLoadLocalFile([NotNullWhen(true)] out StreamReader? reader)
        {
            var inputFile = BuildLocalFilePath();
            if (File.Exists(inputFile))
            {
                reader = new StreamReader(
                    inputFile,
                    new FileStreamOptions
                    {
                        Access = FileAccess.Read,
                        Mode = FileMode.Open,
                    });
                return true;
            }

            reader = null;
            return false;
        }

        private string BuildLocalFilePath()
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LocalCachePath, $"day{contestDay:D2}_input.txt");

        private string BuildRemoteFilePath()
            => $"https://adventofcode.com/{ContestYear}/day/{contestDay}/input";
    }
}