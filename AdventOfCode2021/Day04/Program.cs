using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static class Program
{
    private const int Width = 5;
    private const int Height = 5;

    public static async Task Main()
    {
        var input = await File.ReadAllLinesAsync("input.txt");

        var numbers = input[0]
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        var boards = new List<Board>();

        for (var i = 2; i < input.Length; i += 6)
        {
            var cells = new List<Cell>();
            for (var rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                var row = input[rowIndex + i];
                cells.AddRange(
                    row
                        .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .Select(value => new Cell(value, false))
                );
            }
            
            boards.Add(new Board(cells.ToArray(), Width, Height));
        }

        var game = new Game(boards.ToArray(), numbers);
        var scores = game.Play();

        foreach (var score in scores)
        {
            Console.WriteLine($"Winner score: {score}");
        }
    }
}

public sealed class Cell
{
    public Cell(int value, bool marked)
    {
        Value = value;
        Marked = marked;
    }

    public int Value { get; }
    public bool Marked { get; private set; }

    public void Mark()
    {
        Marked = true;
    }
}

public sealed class Board
{
    private readonly Cell[] _cells;
    private readonly int _width;
    private readonly int _height;

    public Board(Cell[] cells, int width, int height)
    {
        _cells = cells;
        _width = width;
        _height = height;
    }

    public IEnumerable<Cell> GetRow(int index)
        => _cells
            .Skip(index * _width)
            .Take(_width)
            .ToArray();

    public IEnumerable<Cell> GetColumn(int index)
    {
        for (var i = index; i < _cells.Length; i += _width)
        {
            yield return _cells[i];
        }
    }

    public bool IsWinner()
    {
        for (var i = 0; i < _width; i++)
        {
            var column = GetColumn(i);
            if (column.All(c => c.Marked))
            {
                return true;
            }
        }

        for (var i = 0; i < _height; i++)
        {
            var row = GetRow(i);
            if (row.All(c => c.Marked))
            {
                return true;
            }
        }

        return false;
    }

    public int GetScore()
        => _cells
            .Where(c => !c.Marked)
            .Select(c => c.Value)
            .Sum();

    public void Mark(int value)
    {
        foreach (var cell in _cells.Where(c => c.Value == value))
        {
            cell.Mark();
        }
    }
}

public sealed class Game
{
    private readonly Board[] _boards;
    private readonly int[] _input;

    public Game(Board[] boards, int[] input)
    {
        _boards = boards;
        _input = input;
    }

    public IEnumerable<int> Play()
    {
        foreach (var number in _input)
        {
            var boardInGame = _boards
                .Where(b => !b.IsWinner())
                .ToArray();

            if (!boardInGame.Any())
            {
                yield break;
            }
            
            foreach (var board in boardInGame)
            {
                board.Mark(number);
            }

            foreach(var winner in boardInGame.Where(b => b.IsWinner()))
            {
                yield return winner.GetScore() * number;
            }
        }
    }
}