using System;
using System.Collections.Generic;
using System.Linq;

namespace Day23;

public record State(string Rooms, int RoomSize, string Hallway)
{
    private const char Empty = '.';
    private static readonly int[] RoomPositions = { 2, 4, 6, 8 };
    private static readonly Dictionary<char, int> Costs = new() { ['A'] = 1, ['B'] = 10, ['C'] = 100, ['D'] = 1000 };

    public bool IsSolved()
    {
        for (var i = 0; i < 4; i++)
        {
            var roomStart = i * (RoomSize + 1) + 1;
            var roomEnd = roomStart + RoomSize;
            var roomType = roomStart - 1;


            for (var x = roomStart; x < roomEnd; x++)
            {
                if (Rooms[x] != Rooms[roomType])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private string GetRoom(int index)
        => Rooms.Substring(index * (RoomSize + 1), RoomSize + 1);

    private static bool IsRoomEmpty(string room)
        => room.Skip(1).All(c => c == Empty);

    private (string room, char amphipod, int steps) Pop(string room)
    {
        for (var i = 1; i < RoomSize + 1; i++)
        {
            var slot = room[i];

            if (slot == Empty)
            {
                continue;
            }

            if (slot != Empty)
            {
                return (room.Remove(i, 1).Insert(i, "."), slot, i);
            }
        }

        throw new Exception();
    }

    private static bool CanPush(string room)
        => room.Any(c => c == Empty) &&
           room.Skip(1)
               .Where(c => c != Empty)
               .All(c => c == room[0]);

    private (string room, int steps) Push(string room, char amphipod)
    {
        if (room[0] != amphipod)
        {
            throw new Exception();
        }

        if (!CanPush(room))
        {
            throw new Exception();
        }

        for (var i = 1; i < RoomSize + 1; i++)
        {
            if (room[i] == Empty && (i + 1 >= room.Length || room[i + 1] != Empty))
            {
                return (room.Remove(i, 1).Insert(i, amphipod.ToString()), i);
            }
        }

        throw new Exception();
    }

    private string Replace(string rooms, int roomIndex, string room)
    {
        var roomStart = roomIndex * (RoomSize + 1);
        return rooms.Remove(roomStart, RoomSize + 1).Insert(roomStart, room);
    }

    public IEnumerable<(State, int)> GetNextStates()
    {
        for (var roomIndex = 0; roomIndex < 4; roomIndex++)
        {
            var room = GetRoom(roomIndex);
            if (IsRoomEmpty(room))
            {
                continue;
            }

            var (newRoom, amphipod, roomSteps) = Pop(room);

            var suitableRoomResult = FindSuitableRooms(RoomToHallIndex(roomIndex), amphipod);
            if (suitableRoomResult != null)
            {
                var (suitableRoomIndex, stepsBetweenRooms) = suitableRoomResult.Value;
                var goalRoom = GetRoom(suitableRoomIndex);
                if (suitableRoomIndex != roomIndex && CanPush(goalRoom))
                {
                    var (newGoalRoom, goalRoomSteps) = Push(goalRoom, amphipod);
                    var rooms = Replace(Rooms, roomIndex, newRoom);
                    rooms = Replace(rooms, suitableRoomIndex, newGoalRoom);

                    yield return (this with
                    {
                        Rooms = rooms,
                    }, (stepsBetweenRooms + roomSteps + goalRoomSteps) * Costs[amphipod]);
                }
            }

            foreach (var spaceIndex in EnumerateOpenSpace(RoomToHallIndex(roomIndex)))
            {
                yield return (this with
                {
                    Rooms = Replace(Rooms, roomIndex, newRoom),
                    Hallway = Hallway.Remove(spaceIndex, 1).Insert(spaceIndex, amphipod.ToString()),
                }, (Math.Abs(RoomToHallIndex(roomIndex) - spaceIndex) + roomSteps) * Costs[amphipod]);
            }
        }

        for (var hallIndex = 0; hallIndex < Hallway.Length; hallIndex++)
        {
            var amphipod = Hallway[hallIndex];
            if (amphipod == Empty)
            {
                continue;
            }

            foreach (var (roomIndex, newHallIndex) in EnumerateAvailableRooms(hallIndex))
            {
                var room = GetRoom(roomIndex);
                if (room[0] == amphipod && CanPush(room))
                {
                    var (newRoom, roomSteps) = Push(room, amphipod);

                    yield return (this with
                    {
                        Rooms = Replace(Rooms, roomIndex, newRoom),
                        Hallway = Hallway.Remove(hallIndex, 1).Insert(hallIndex, "."),
                    }, (Math.Abs(newHallIndex - hallIndex) + roomSteps) * Costs[amphipod]);
                }
            }
        }
    }

    private static int RoomToHallIndex(int roomIndex)
        => roomIndex * 2 + 2;

    private (int suitableRoomIndex, int stepsBetweenRooms)? FindSuitableRooms(int hallIndex, char amphipod)
    {
        var roomIndex = amphipod - 'A';
        var goalHallIndex = RoomToHallIndex(roomIndex);

        var min = Math.Min(hallIndex, goalHallIndex);
        var max = Math.Max(hallIndex, goalHallIndex) + 1;
        for (var i = min; i < max; i++)
        {
            if (Hallway[i] != Empty)
            {
                return null;
            }
        }

        return (roomIndex, max - min - 1);
    }

    private IEnumerable<(int roomIndex, int roomHallIndex)> EnumerateAvailableRooms(int hallIndex)
    {
        for (var i = hallIndex + 1; i < Hallway.Length; i++)
        {
            if (RoomPositions.Contains(i))
            {
                yield return (Array.IndexOf(RoomPositions, i), i);
                continue;
            }

            if (Hallway[i] != Empty)
            {
                break;
            }
        }

        for (var i = hallIndex - 1; i >= 0; i--)
        {
            if (Hallway[i] != Empty)
            {
                break;
            }

            if (RoomPositions.Contains(i))
            {
                yield return (Array.IndexOf(RoomPositions, i), i);
            }
        }
    }

    private IEnumerable<int> EnumerateOpenSpace(int roomHallIndex)
    {
        for (var i = roomHallIndex - 1; i >= 0 && Hallway[i] == Empty; i--)
        {
            if (!RoomPositions.Contains(i))
            {
                yield return i;
            }
        }

        for (var i = roomHallIndex + 1; i < Hallway.Length && Hallway[i] == Empty; i++)
        {
            if (!RoomPositions.Contains(i))
            {
                yield return i;
            }
        }
    }
}