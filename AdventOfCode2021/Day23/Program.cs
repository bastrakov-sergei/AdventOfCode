using Day23;

Part1();
Part2();

//Example debug
/*var x1 = initialState.GetNextStates();
var (step1, e1) = x1.First(current => current.Item1.Rooms == "ABABCDC.CDDA" && current.Item1.Hallway == "...B.......");
Console.WriteLine($"{step1}");
var x2 = step1.GetNextStates();
var (step2, e2) = x2.First(current => current.Item1.Rooms == "ABAB.DCCCDDA" && current.Item1.Hallway == "...B.......");
Console.WriteLine($"{step2}");
var x3 = step2.GetNextStates();
var (step3, e3) = x3.First(current => current.Item1.Rooms == "ABAB..CCCDDA" && current.Item1.Hallway == "...B.D.....");
Console.WriteLine($"{step3}");
var x4 = step3.GetNextStates();
var (step4, e4) = x4.First(current => current.Item1.Rooms == "ABAB.BCCCDDA" && current.Item1.Hallway == ".....D.....");
Console.WriteLine($"{step4}");
var x5 = step4.GetNextStates();
var (step5, e5) = x5.First(current => current.Item1.Rooms == "A.ABBBCCCDDA" && current.Item1.Hallway == ".....D.....");
Console.WriteLine($"{step5}");
var x6 = step5.GetNextStates();
var (step6, e6) = x6.First(current => current.Item1.Rooms == "A.ABBBCCCD.A" && current.Item1.Hallway == ".....D.D..." );
Console.WriteLine($"{step6}");
var x7 = step6.GetNextStates();
var (step7, e7) = x7.First(current => current.Item1.Rooms == "A.ABBBCCCD.." && current.Item1.Hallway == ".....D.D.A.");
Console.WriteLine($"{step7}");
var x8 = step7.GetNextStates();
var (step8, e8) = x8.First(current => current.Item1.Rooms == "A.ABBBCCCD.D" && current.Item1.Hallway == ".....D...A.");
Console.WriteLine($"{step8}");
var x9 = step8.GetNextStates();
var (step9, e9) = x9.First(current => current.Item1.Rooms == "A.ABBBCCCDDD" && current.Item1.Hallway == ".........A.");
Console.WriteLine($"{step9}");
var x10 = step9.GetNextStates();
var (step10, e10) = x10.First(current => current.Item1.Rooms == "AAABBBCCCDDD" && current.Item1.Hallway == "...........");
Console.WriteLine($"{step10} {step10.IsSolved()} {e1+e2+e3+e4+e5+e6+e7+e8+e9+e10}");
*/

/*
 * #############
 * #...........#
 * ###B#D#C#A###
 *   #C#D#B#A#
 *   #########
 */
void Part1()
{
    var initialState = new State("ABCBDDCCBDAA", 2, "...........");
    Console.WriteLine($"Part 1: {Solve(initialState)}");
}

/*
 * #############
 * #...........#
 * ###B#D#C#A###
 *   #D#C#B#A#
 *   #D#B#A#C#
 *   #C#D#B#A#
 *   #########
 */
void Part2()
{
    var initialState = new State("ABDDCBDCBDCCBABDAACA", 4, "...........");
    Console.WriteLine($"Part 2: {Solve(initialState)}");
}

(State? state, int consumedEnergy) Solve(State state)
{
    var queue = new PriorityQueue<State, int>();
    var distances = new Dictionary<State, int>();
    var visited = new HashSet<State>();

    distances.Add(state, 0);
    queue.Enqueue(state, 0);

    while (queue.TryDequeue(out var current, out var consumedEnergy))
    {
        if (current.IsSolved())
        {
            return (current, consumedEnergy);
        }

        if (visited.Contains(current))
        {
            continue;
        }

        visited.Add(current);

        foreach (var (next, stepEnergy) in current.GetNextStates())
        {
            var nextCost = consumedEnergy + stepEnergy;

            if (nextCost < distances.GetValueOrDefault(next, int.MaxValue))
            {
                distances[next] = nextCost;
                queue.Enqueue(next, nextCost);
            }
        }
    }

    return (null, 0);
}