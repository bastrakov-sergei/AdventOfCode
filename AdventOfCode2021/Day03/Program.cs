using System;
using System.IO;
using System.Linq;

var input = await File.ReadAllLinesAsync("input.txt");

var gamma = "";
var epsilon = "";

for (var i = 0; i < input[0].Length; i++)
{
    gamma += GetCommonBit(i, input, true);
    epsilon += GetCommonBit(i, input, false);
}

var gammaNumber = ConvertToInt(gamma);
var epsilonNumber = ConvertToInt(epsilon);

Console.WriteLine($"gamma: {gammaNumber}, epsilon: {epsilonNumber}, result: {gammaNumber * epsilonNumber}");

var oxygenNumbers = input;
var index = 0;
while (oxygenNumbers.Length > 1 && index < oxygenNumbers[0].Length)
{
    var mostCommonBit = GetCommonBit(index, oxygenNumbers, true);
    oxygenNumbers = oxygenNumbers
        .Where(number => number[index] == mostCommonBit)
        .ToArray();
    index++;
}

var oxygen = oxygenNumbers.First();

var coNumbers = input;
index = 0;
while (coNumbers.Length > 1 && index < coNumbers[0].Length)
{
    var mostCommonBit = GetCommonBit(index, coNumbers, false);
    coNumbers = coNumbers
        .Where(number => number[index] == mostCommonBit)
        .ToArray();
    index++;
}

var co = coNumbers.First();

var oxygenNumber = ConvertToInt(oxygen);
var coNumber = ConvertToInt(co);

Console.WriteLine($"oxygen: {oxygenNumber}, co: {coNumber}, result: {oxygenNumber * coNumber}");

char GetCommonBit(int index, string[] numbers, bool most)
{
    var zeros = 0;
    var ones = 0;

    foreach (var number in numbers)
    {
        switch (number[index])
        {
            case '0':
                zeros++;
                break;
            case '1':
                ones++;
                break;
        }
    }

    if (ones == zeros)
    {
        if (most)
        {
            return '1';
        }

        return '0';
    }

    if (zeros > ones)
    {
        return most ? '0' : '1';
    }

    return most ? '1' : '0';
}

int ConvertToInt(string s)
{
    var result = 0;

    for (var i = 0; i < s.Length; i++)
    {
        var j = s.Length - i - 1;
        result += (int) Math.Pow(2, i) * (s[j] - '0');
    }

    return result;
}