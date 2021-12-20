public class Scanner
{
    public string Name { get; }
    public Vector3 Position { get; }
    public HashSet<Vector3> Beacons { get; }
    public HashSet<Vector3> AbsoluteBeacons { get; }

    public Scanner(string name, Vector3 position, IEnumerable<Vector3> beacons)
    {
        Name = name;
        Position = position;
        Beacons = beacons.ToHashSet();
        AbsoluteBeacons = Beacons.Select(b => b + Position).ToHashSet();
    }

    public bool TryAlign(Scanner other, out Scanner aligned)
    {
        foreach (var myBeacon in AbsoluteBeacons)
        {
            foreach (var otherBeacon in other.Beacons)
            {
                    var offset = myBeacon - otherBeacon;

                    var matches = 0;
                    foreach (var shiftedBeacon in other.Beacons.Select(b => b + offset))
                    {
                        if (AbsoluteBeacons.Contains(shiftedBeacon))
                        {
                            matches++;
                            if (matches > 11)
                            {
                                break;
                            }
                        }
                    }

                    if (matches > 11)
                    {
                        aligned = new Scanner(other.Name, offset, other.Beacons);
                        return true;
                    }
            }
        }

        aligned = other;
        return false;
    }

    public override string ToString()
        => $"(Name={Name},Position={Position.ToString()},Beacons=[{string.Join(",\n", Beacons)}])";
}