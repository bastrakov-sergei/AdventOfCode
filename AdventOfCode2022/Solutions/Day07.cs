using System;
using System.Collections.Generic;
using System.Linq;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022.Solutions;

public class Day07 : AdventOfCodeBaseSolution
{
    public Day07() : base()
    {
    }

    [TestCase(Part.Part1, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, TestName = "Part 2 solution")]
    public void Solve(Part part)
    {
        var input = Input.ReadAllLines().ToArray();
        PrintSolution(part, () => SolvePart1(input), () => SolvePart2(input));
    }

    [TestCase(ExpectedResult = "95437")]
    public string SolveTestPart1()
    {
        var input = new[]
        {
            "$ cd /", "$ ls", "dir a", "14848514 b.txt", "8504156 c.dat", "dir d", "$ cd a", "$ ls", "dir e", "29116 f",
            "2557 g", "62596 h.lst", "$ cd e", "$ ls", "584 i", "$ cd ..", "$ cd ..", "$ cd d", "$ ls", "4060174 j",
            "8033020 d.log", "5626152 d.ext", "7214296 k",
        };

        return SolvePart1(input);
    }

    [TestCase(ExpectedResult = "24933642")]
    public string SolveTestPart2()
    {
        var input = new[]
        {
            "$ cd /", "$ ls", "dir a", "14848514 b.txt", "8504156 c.dat", "dir d", "$ cd a", "$ ls", "dir e", "29116 f",
            "2557 g", "62596 h.lst", "$ cd e", "$ ls", "584 i", "$ cd ..", "$ cd ..", "$ cd d", "$ ls", "4060174 j",
            "8033020 d.log", "5626152 d.ext", "7214296 k",
        };

        return SolvePart2(input);
    }

    private static string SolvePart1(string[] input)
    {
        var root = ParseInput(input);
        var sizes = root
            .Enumerate()
            .Where(d => d.Size < 1_000_000)
            .Select(d => d.Size)
            .ToArray();
        return sizes.Sum().ToString();
    }

    private static string SolvePart2(string[] input)
    {
        var root = ParseInput(input);
        var neededSize = root.Size - (70_000_000 - 30_000_000);

        var sizes = root
            .Enumerate()
            .Where(d => d.Size > neededSize)
            .Select(d => d.Size)
            .ToArray();

        return sizes.Min().ToString();
    }

    private static Directory ParseInput(string[] input)
    {
        var root = new Directory("/", null);
        var current = root;

        foreach (var line in input)
        {
            var words = line.Split();
            switch (words[0])
            {
                case "$":
                    switch (words[1])
                    {
                        case "ls":
                            break;
                        case "cd":
                            current = current.FindDirectory(words[2]);
                            break;
                        default:
                            throw new Exception("Unknown command");
                    }

                    break;
                case "dir":
                    current.Add(new Directory(words[1], current));
                    break;
                default:
                    current.Add(new File(words[1], int.Parse(words[0])));
                    break;
            }
        }

        return root;
    }

    public record Directory(string Name, Directory? Parent)
    {
        private List<Directory> Directories { get; } = new();
        private List<File> Files { get; } = new();

        public int Size
            => Files.Select(f => f.Size).Sum() +
               Directories.Select(d => d.Size).Sum();

        public void Add(File file)
            => Files.Add(file);

        public void Add(Directory directory)
            => Directories.Add(directory);

        public Directory FindDirectory(string name)
        {
            return name switch
            {
                "/" => FindRoot(),
                ".." => Parent ?? this,
                _ => Directories.First(d => d.Name == name),
            };
        }

        public IEnumerable<Directory> Enumerate()
        {
            yield return this;

            foreach (var child in Directories.SelectMany(directory => directory.Enumerate()))
            {
                yield return child;
            }
        }

        private Directory FindRoot()
        {
            var root = this;
            while (root.Parent != null)
            {
                root = root.Parent;
            }

            return root;
        }

        public override string ToString()
            => Print();

        private string Print(string? tab = null)
        {
            tab ??= string.Empty;
            return $"{tab}- {Name} (dir, size={Size})\n" +
                   string.Join("", Directories.Select(d => d.Print(tab + "  ")).ToArray()) +
                   string.Join("", Files.Select(f => f.Print(tab + "  ")).ToArray());
        }
    }

    public record File(string Name, int Size)
    {
        public string Print(string? tab = null)
        {
            tab ??= string.Empty;
            return $"{tab}- {Name} (file, size={Size})\n";
        }
    }
}