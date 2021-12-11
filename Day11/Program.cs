var field = File
    .ReadAllLines("input.txt")
    .Select(line => line.Select(c => c - '0').ToArray())
    .ToArray();

var width = field[0].Length;
var height = field.Length;

(int x, int y)[] neighborOffsets =
{
    (x: 0, y: 1),
    (x: 1, y: 1),
    (x: 1, y: 0),
    (x: 1, y: -1),
    (x: 0, y: -1),
    (x: -1, y: -1),
    (x: -1, y: 0),
    (x: -1, y: 1),
};

PrintField();

var flashes = 0;
for (var step = 1; step < 1000; step++)
{
    //increase energy step
    foreach (var point in EnumeratePoints())
    {
        field[point.x][point.y]++;
    }

    //increase neighbors energy step
    var queue = new Queue<(int x, int y)>();
    var visited = new HashSet<(int x, int y)>();

    foreach (var point in EnumeratePoints())
    {
        if (field[point.x][point.y] > 9)
        {
            queue.Enqueue(point);
        }
    }

    while (queue.TryDequeue(out var point))
    {
        visited.Add(point);

        foreach (var neighbor in GetNeighbors(point))
        {
            if (visited.Contains(neighbor))
            {
                continue;
            }

            field[neighbor.x][neighbor.y]++;

            if (field[neighbor.x][neighbor.y] > 9 && !queue.Contains(neighbor))
            {
                queue.Enqueue(neighbor);
            }
        }
    }

    var stepFlashes = 0;
    //flash step
    foreach (var point in EnumeratePoints())
    {
        if (field[point.x][point.y] > 9)
        {
            stepFlashes++;
            field[point.x][point.y] = 0;
        }
    }

    if (field.SelectMany(x => x).All(x => x == 0))
    {
        Console.WriteLine($"All flash at {step}");
        break;
    }

    flashes += stepFlashes;
    Console.WriteLine($"Step {step}: {stepFlashes}, Total: {flashes}");
}

Console.WriteLine($"Total flashes: {flashes}");

void PrintField()
{
    Console.WriteLine("------------------------------");
    for (var x = 0; x < width; x++)
    {
        for (var y = 0; y < height; y++)
        {
            Console.Write(field[x][y]);
        }

        Console.WriteLine();
    }

    Console.WriteLine("------------------------------");
}

IEnumerable<(int x, int y)> EnumeratePoints()
    => Enumerable
        .Range(0, width)
        .SelectMany(x => Enumerable.Range(0, height).Select(y => (x, y)));

IEnumerable<(int x, int y)> GetNeighbors((int x, int y) coordinate)
    => neighborOffsets
        .Select(offset => (x: coordinate.x + offset.x, y: coordinate.y + offset.y))
        .Where(neighbor => neighbor.x >= 0)
        .Where(neighbor => neighbor.x < width)
        .Where(neighbor => neighbor.y >= 0)
        .Where(neighbor => neighbor.y < height);