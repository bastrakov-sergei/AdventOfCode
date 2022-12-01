using System;
using System.IO;
using System.Linq;
using Day16;
using Newtonsoft.Json;

var input = File.ReadAllText("input.txt");
var result = input.Parse();

Console.WriteLine(JsonConvert.SerializeObject(result));
Console.WriteLine(result.ToPrettyString());

Console.WriteLine($"Part 1: {SumVersion(result)}");
Console.WriteLine($"Part 2: {result.GetValue()}");

int SumVersion(Packet packet)
    => packet switch
    {
        LiteralPacket l => l.Version,
        OperatorPacket p => p.Version + p.Packets.Select(SumVersion).Sum(),
        _ => throw new ArgumentOutOfRangeException(nameof(packet), packet, null),
    };