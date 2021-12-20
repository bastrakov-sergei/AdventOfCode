using System.Collections.Immutable;

public static class Parser
{
    public static Scanner[] Parse(this string[] input)
    {
        var scanners = new List<Scanner>();
        var beacons = new List<Vector3>();
        var scannerName = "";

        foreach (var line in input)
        {
            if (line.StartsWith("---"))
            {
                scannerName = line.Split("---", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[0];
                beacons.Clear();
                continue;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                scanners.AddRange(CreateWithRotations(scannerName, beacons));
                continue;
            }

            var coordinates = line
                .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            beacons.Add(new Vector3(coordinates[0], coordinates[1], coordinates[2]));
        }

        scanners.AddRange(CreateWithRotations(scannerName, beacons));

        return scanners.ToArray();
    }

    private static IEnumerable<Scanner> CreateWithRotations(string scannerName, IEnumerable<Vector3> beacons)
        => Vector3.Rotations.Select(rotation =>
            new Scanner(scannerName, Vector3.Zero, beacons.Select(rotation).ToImmutableHashSet()));
}