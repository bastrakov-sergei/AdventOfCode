var input = File
    .ReadAllLines("input.txt")
    .ToArray();

var caves = new HashSet<string>();
var neighbors = new Dictionary<string, string[]>();

foreach (var line in input)
{
    var parts = line.Split("-", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    caves.Add(parts[0]);
    caves.Add(parts[1]);

    neighbors[parts[0]] = (neighbors.GetValueOrDefault(parts[0]) ?? Array.Empty<string>()).Concat(new[] { parts[1] })
        .ToArray();
    neighbors[parts[1]] = (neighbors.GetValueOrDefault(parts[1]) ?? Array.Empty<string>()).Concat(new[] { parts[0] })
        .ToArray();
}

Part1();
Part2();


bool IsSmallCave(string cave)
    => cave.All(char.IsLower);

void Part1()
{
    var queue = new Queue<(string cave, string[] path)>();
    queue.Enqueue(("start", new[] { "start" }));

    var pathsCount = 0;
    while (queue.TryDequeue(out var current))
    {
        var (cave, path) = current;

        foreach (var neighbor in neighbors[cave])
        {
            if (neighbor == "end")
            {
                pathsCount++;
            }
            else if (IsSmallCave(neighbor))
            {
                if (path.All(x => x != neighbor))
                {
                    queue.Enqueue((neighbor, path.Append(neighbor).ToArray()));
                }
            }
            else
            {
                queue.Enqueue((neighbor, path.Append(neighbor).ToArray()));
            }
        }
    }

    Console.WriteLine($"Part 1: {pathsCount}");
}

bool HaveDoubleVisit(string[] visited)
    => visited
        .Where(IsSmallCave)
        .GroupBy(cave => cave)
        .Any(group => group.Count() == 2);

void Part2()
{
    var queue = new Queue<(string cave, string[] path)>();
    queue.Enqueue(("start", new[] { "start" }));

    var pathsCount = 0;
    while (queue.TryDequeue(out var current))
    {
        var (cave, path) = current;
        //Console.WriteLine(string.Join(",", path));

        foreach (var neighbor in neighbors[cave])
        {
            if (neighbor == "end")
            {
                //Console.WriteLine(string.Join(",", path.Append(neighbor)));
                pathsCount++;
            }
            else if (IsSmallCave(neighbor) && neighbor != "start")
            {
                if (HaveDoubleVisit(path))
                {
                    if (!path.Contains(neighbor))
                    {
                        queue.Enqueue((neighbor, path.Append(neighbor).ToArray()));
                    }
                }
                else
                {
                    queue.Enqueue((neighbor, path.Append(neighbor).ToArray()));
                }
            }
            else if (neighbor != "start")
            {
                queue.Enqueue((neighbor, path.Append(neighbor).ToArray()));
            }
        }
    }

    Console.WriteLine($"Part 2: {pathsCount}");
}