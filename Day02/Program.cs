using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static class Program
{
    public static async Task Main()
    {
        var input = await File.ReadAllTextAsync("input1.txt");
        var commands = Parse(input);
        var submarine = commands.Aggregate(new Submarine(0, 0, 0), (x, command) => command.Move(x));
        
        Console.WriteLine(submarine.Position * submarine.Depth);
    }

    public static MoveCommand[] Parse(string input) 
        => input
            .Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(tokens => (MoveCommand) (tokens[0] switch
            {
                "forward" => new ForwardCommand(int.Parse(tokens[1])),
                "up" => new UpCommand(int.Parse(tokens[1])),
                "down" => new DownCommand(int.Parse(tokens[1])),
                _ => throw new InvalidOperationException()
            }))
            .ToArray();
}