using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var players = File
    .ReadAllLines("input.txt")
    .Select(line => line.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
    .Select(parts => parts[^1])
    .Select(int.Parse)
    .Select(position => new Pawn(position, 0))
    .ToArray();

var cache = new Dictionary<(Pawn Pawn1, Pawn Pawn2), (long CountP1, long CountP2)>();

Part1(players.ToArray());
Part2(players.ToArray());

void Part1(Pawn[] pawns)
{
    var rollCount = 0;
    var dieScore = 0;
    var currentPlayerIndex = 0;

    Pawn winner;
    while (true)
    {
        var turnScore = Roll() + Roll() + Roll();
        var newPosition = (pawns[currentPlayerIndex].Position + turnScore - 1) % 10 + 1;

        pawns[currentPlayerIndex] = pawns[currentPlayerIndex] with
        {
            Position = newPosition,
            Score = pawns[currentPlayerIndex].Score + newPosition,
        };

        if (pawns[currentPlayerIndex].Score >= 1000)
        {
            winner = pawns[currentPlayerIndex];
            break;
        }

        currentPlayerIndex = 1 - currentPlayerIndex;
    }

    var looser = pawns.First(pawn => pawn != winner);
    var result = looser.Score * rollCount;

    Console.WriteLine($"Part 1: {result}");

    int Roll()
    {
        dieScore++;
        if (dieScore > 100)
        {
            dieScore = 1;
        }
        rollCount++;
        return dieScore;
    }
}
void Part2(Pawn[] pawns)
{
    var result = CalculateWins(pawns[0], pawns[1]);
    Console.WriteLine($"Part 2: {result}");
}

(long pawn1Wins, long pawn2Wins) CalculateWins(Pawn pawn1, Pawn pawn2)
{
    if (pawn1.Score >= 21)
    {
        return (1, 0);
    }

    if (pawn2.Score >= 21)
    {
        return (0, 1);
    }

    if (cache.TryGetValue((pawn1, pawn2), out var result))
    {
        return result;
    }

    var totalWinP1 = 0L;
    var totalWinP2 = 0L;

    for (var i = 1; i <= 3; i++)
    {
        for (var j = 1; j <= 3; j++)
        {
            for (var k = 1; k <= 3; k++)
            {
                var newPositionP1 = (pawn1.Position + i + j + k - 1) % 10 + 1;
                var newScoreP1 = pawn1.Score + newPositionP1;

                var (winCountP2, winCountP1) =
                    CalculateWins(pawn2, pawn1 with
                    {
                        Position = newPositionP1,
                        Score = newScoreP1,
                    });
                totalWinP1 += winCountP1;
                totalWinP2 += winCountP2;
            }
        }
    }

    cache[(pawn1, pawn2)] = (totalWinP1, totalWinP2);
    return (totalWinP1, totalWinP2);
}

public record Pawn(int Position, int Score);