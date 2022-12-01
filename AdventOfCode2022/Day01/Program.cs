var caloriesPerElve = SelectTotalCaloriesPerElve(await File.ReadAllLinesAsync("input1.txt")).ToArray();

Console.WriteLine($"Solution 1: {caloriesPerElve.Max()}");
Console.WriteLine($"Solution 2: {caloriesPerElve.OrderByDescending(i => i).Take(3).Sum()}");

IEnumerable<int> SelectTotalCaloriesPerElve(IEnumerable<string> lines)
{
    var sum = 0;
    foreach (var line in lines)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            yield return sum;
            sum = 0;
        }
        else
        {
            sum += int.Parse(line);
        }
    }
}