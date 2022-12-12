using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Flurl.Http;

namespace AdventOfCode2022;

public static class InputCache
{
    private const string LocalCachePath = "../../../inputCache";

    private static int ContestYear => int.TryParse(Environment.GetEnvironmentVariable("ContestYear"), out var year)
        ? year
        : DateTime.Now.Year;

    public static async Task<StreamReader> GetInputAsync(int contestDay)
    {
        if (TryLoadLocalFile(contestDay, out var streamReader))
        {
            return streamReader;
        }

        return await LoadRemoteFileAsync(contestDay);
    }

    private static async Task<StreamReader> LoadRemoteFileAsync(int contestDay)
    {
        var session = Environment.GetEnvironmentVariable("AdventOfCodeSession")
                      ?? throw new InvalidOperationException("Session cookie not specified");
        var rawBytes = await BuildRemoteFilePath(contestDay).WithCookie("session", session).GetBytesAsync();
        var path = BuildLocalFilePath(contestDay);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        await File.WriteAllBytesAsync(path, rawBytes);

        if (TryLoadLocalFile(contestDay, out var reader))
        {
            return reader;
        }

        throw new Exception("Some shit happened");
    }

    private static bool TryLoadLocalFile(int contestDay, [NotNullWhen(true)] out StreamReader? reader)
    {
        var inputFile = BuildLocalFilePath(contestDay);
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

    private static string BuildLocalFilePath(int contestDay)
        => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LocalCachePath, $"day{contestDay:D2}_input.txt");

    private static string BuildRemoteFilePath(int contestDay)
        => $"https://adventofcode.com/{ContestYear}/day/{contestDay}/input";
}