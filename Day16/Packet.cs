namespace Day16;

public abstract class Packet
{
    public int Version { get; }

    protected Packet(int version)
        => Version = version;

    public abstract long GetValue();

    public abstract string ToPrettyString();
}