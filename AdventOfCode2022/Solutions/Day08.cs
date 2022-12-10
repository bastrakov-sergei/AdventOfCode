using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AOCCommon;
using NUnit.Framework;

namespace AdventOfCode2022.Solutions;

public class Day08 : AdventOfCodeBaseSolution
{
    public Day08() : base(8)
    {
    }

    [TestCase(Part.Part1, TestName = "Part 1 solution")]
    [TestCase(Part.Part2, TestName = "Part 2 solution")]
    public void Solve(Part part)
    {
        var input = Input.ReadAllLines().ToArray();
        var matrix = ParseMatrix(input);
        PrintSolution(part, () => SolvePart1(matrix), () => SolvePart2(matrix));
    }

    [TestCase(ExpectedResult = "21")]
    public string SolveTestPart1()
    {
        var input = new[]
        {
            "30373",
            "25512",
            "65332",
            "33549",
            "35390",
        };
        var matrix = ParseMatrix(input);

        return SolvePart1(matrix);
    }

    [TestCase(ExpectedResult = "8")]
    public string SolveTestPart2()
    {
        var input = new[]
        {
            "30373",
            "25512",
            "65332",
            "33549",
            "35390",
        };
        var matrix = ParseMatrix(input);

        return SolvePart2(matrix);
    }

    private static string SolvePart1(Matrix<int> matrix)
    {
        var count = 0;

        for (var y = 1; y < matrix.Height - 1; y++)
        {
            for (var x = 1; x < matrix.Width - 1; x++)
            {
                var upRay = matrix.Raycast(x, y, Direction.Up).ToArray();
                if (IsVisibleFromEdge(upRay))
                {
                    count++;
                    continue;
                }

                var downRay = matrix.Raycast(x, y, Direction.Down).ToArray();
                if (IsVisibleFromEdge(downRay))
                {
                    count++;
                    continue;
                }

                var leftRay = matrix.Raycast(x, y, Direction.Left).ToArray();
                if (IsVisibleFromEdge(leftRay))
                {
                    count++;
                    continue;
                }

                var rightRay = matrix.Raycast(x, y, Direction.Right).ToArray();
                if (IsVisibleFromEdge(rightRay))
                {
                    count++;
                }
            }
        }

        count += (matrix.Width + matrix.Height - 2) * 2;
        return count.ToString();
    }

    private static bool IsVisibleFromEdge(IReadOnlyList<int> ray)
    {
        for (var i = 1; i < ray.Count; i++)
        {
            if (ray[i] >= ray[0])
            {
                return false;
            }
        }

        return true;
    }

    private static string SolvePart2(Matrix<int> matrix)
    {
        var maxScore = 0;

        for (var y = 1; y < matrix.Height - 1; y++)
        {
            for (var x = 1; x < matrix.Width - 1; x++)
            {
                var treeScore = 1;
                var upRay = matrix.Raycast(x, y, Direction.Up).ToArray();
                var downRay = matrix.Raycast(x, y, Direction.Down).ToArray();
                var leftRay = matrix.Raycast(x, y, Direction.Left).ToArray();
                var rightRay = matrix.Raycast(x, y, Direction.Right).ToArray();
                
                treeScore *= GetVisibleTrees(upRay);
                treeScore *= GetVisibleTrees(downRay);
                treeScore *= GetVisibleTrees(leftRay);
                treeScore *= GetVisibleTrees(rightRay);

                maxScore = Math.Max(maxScore, treeScore);
            }
        }

        return maxScore.ToString();
    }
    
    private static int GetVisibleTrees(IReadOnlyList<int> ray)
    {
        for (var i = 1; i < ray.Count; i++)
        {
            if (ray[i] >= ray[0])
            {
                return i;
            }
        }

        return ray.Count - 1;
    }

    private static Matrix<int> ParseMatrix(IReadOnlyList<string> input)
    {
        var (width, height) = (input[0].Length, input.Count);
        var matrix = new Matrix<int>(input[0].Length, input.Count);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                matrix[x, y] = input[y][x] - '0';
            }
        }

        return matrix;
    }

    public class Matrix<T>
    {
        public int Width { get; }
        public int Height { get; }

        private readonly T[] data;

        public Matrix(int width, int height)
        {
            Width = width;
            Height = height;
            data = new T[width * height];
        }

        public T this[int x, int y]
        {
            get => data[y * Width + x];
            set => data[y * Width + x] = value;
        }

        public IEnumerable<T> Raycast(int x, int y, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    for (; y >= 0; y--)
                    {
                        yield return this[x, y];
                    }

                    break;
                case Direction.Down:
                {
                    for (; y < Height; y++)
                    {
                        yield return this[x, y];
                    }

                    break;
                }
                case Direction.Left:
                    for (; x >= 0; x--)
                    {
                        yield return this[x, y];
                    }

                    break;
                case Direction.Right:
                    for (; x < Width; x++)
                    {
                        yield return this[x, y];
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    sb.Append(this[x, y]);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
    
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }
}