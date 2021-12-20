var scanners = File
    .ReadAllLines("input.txt")
    .Parse()
    .ToArray();

var unalignedScanners = scanners
    .GroupBy(scanner => scanner.Name)
    .ToDictionary(g => g.Key, g => g.ToArray());

var alignedScanners = new Dictionary<string, Scanner>();
var (firstName, rotatedScanners) = unalignedScanners.First();
alignedScanners.Add(firstName, rotatedScanners.First());
unalignedScanners.Remove(firstName);

var queue = new Queue<string>();
queue.Enqueue(firstName);

while (queue.TryDequeue(out var currentName))
{
    var current = alignedScanners[currentName];
    var aligned = unalignedScanners
        .Values
        .Select(anyRotationScannerGroup => anyRotationScannerGroup
            .Select(s => (IsAligned: current.TryAlign(s, out var aligned), AlignedScanner: aligned))
            .FirstOrDefault(x => x.IsAligned)
        )
        .Where(x => x.IsAligned)
        .Select(x => x.AlignedScanner);

    foreach (var scanner in aligned)
    {
        Console.WriteLine($"Aligned: {scanner.Name} with {current.Name}. Total: {alignedScanners.Count}");
        alignedScanners[scanner.Name] = scanner;
        queue.Enqueue(scanner.Name);
        unalignedScanners.Remove(scanner.Name);
    }
}

var totalBeacons = alignedScanners.Values.SelectMany(s => s.AbsoluteBeacons).ToHashSet();
Console.WriteLine($"Part 1: {totalBeacons.Count}");

var scannersArray = alignedScanners.Values.ToArray();
Console.WriteLine($"Part 2: {scannersArray.SelectMany((a, i) => scannersArray[i..].Select(b => GetManhattanDistance(a.Position, b.Position))).Max()}");

int GetManhattanDistance(Vector3 a, Vector3 b)
{
    var vector = a - b;
    return Math.Abs(vector.X) + Math.Abs(vector.Y) + Math.Abs(vector.Z);
}