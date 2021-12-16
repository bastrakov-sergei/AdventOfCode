using Newtonsoft.Json;

namespace Day16;

public abstract class Packet
{
    public int Version { get; }

    public Packet(int version)
    {
        Version = version;
    }

    public abstract long GetValue();

    public override string ToString()
        => $"{GetType().Name}: {JsonConvert.SerializeObject(this)}";
}