using System;

Console.WriteLine("Part 1: 39494195799979");
Console.WriteLine("Part 2: 13161151139617");
/*
inp w       inp w       inp w       inp w       inp w       inp w       inp w       inp w       inp w       inp w       inp w       inp w       inp w       inp w
mul x 0     mul x 0     mul x 0     mul x 0     mul x 0     mul x 0     mul x 0     mul x 0     mul x 0     mul x 0     mul x 0     mul x 0     mul x 0     mul x 0
add x z     add x z     add x z     add x z     add x z     add x z     add x z     add x z     add x z     add x z     add x z     add x z     add x z     add x z
mod x 26    mod x 26    mod x 26    mod x 26    mod x 26    mod x 26    mod x 26    mod x 26    mod x 26    mod x 26    mod x 26    mod x 26    mod x 26    mod x 26
div z 1     div z 1     div z 1     div z 1     div z 26    div z 1     div z 1     div z 26    div z 1     div z 26    div z 26    div z 26    div z 26    div z 26
add x 13    add x 15    add x 15    add x 11    add x -7    add x 10    add x 10    add x -5    add x 15    add x -3    add x 0     add x -5    add x -9    add x 0
eql x w     eql x w     eql x w     eql x w     eql x w     eql x w     eql x w     eql x w     eql x w     eql x w     eql x w     eql x w     eql x w     eql x w
eql x 0     eql x 0     eql x 0     eql x 0     eql x 0     eql x 0     eql x 0     eql x 0     eql x 0     eql x 0     eql x 0     eql x 0     eql x 0     eql x 0
mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0
add y 25    add y 25    add y 25    add y 25    add y 25    add y 25    add y 25    add y 25    add y 25    add y 25    add y 25    add y 25    add y 25    add y 25
mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x
add y 1     add y 1     add y 1     add y 1     add y 1     add y 1     add y 1     add y 1     add y 1     add y 1     add y 1     add y 1     add y 1     add y 1
mul z y     mul z y     mul z y     mul z y     mul z y     mul z y     mul z y     mul z y     mul z y     mul z y     mul z y     mul z y     mul z y     mul z y
mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0     mul y 0
add y w     add y w     add y w     add y w     add y w     add y w     add y w     add y w     add y w     add y w     add y w     add y w     add y w     add y w
add y 6     add y 7     add y 10    add y 2     add y 15    add y 8     add y 1     add y 10    add y 5     add y 3     add y 5     add y 11    add y 12    add y 10
mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x     mul y x
add z y     add z y     add z y     add z y     add z y     add z y     add z y     add z y     add z y     add z y     add z y     add z y     add z y     add z y

xOffset = 13    15    15    11    -7    10    10    -5    15    -3    0     -5    -9    0
yOffset = 6     7     10    2     15    8     1     10    5     3     5     11    12    10
div     = 1     1     1     1     26    1     1     26    1     26    26    26    26    26

                                                        w   x   y   z
inp w                   w = n[i]                    //  1   0   0   0
mul x 0                 x = x * 0                   //  1   0   0   0
add x z                 x = x + z                   //  1   0   0   0
mod x 26                x = x % 26                  //  1   0   0   0
div z 1                 z = z / div                 //  1   0   0   0
add x 13                x = x + xOffset             //  1  13   0   0
eql x w                 x = x == w ? 1 : 0          //  1   0   0   0
eql x 0                 x = x == 0 ? 1 : 0          //  1   1   0   0
mul y 0                 y = y * 0                   //  1   1   0   0
add y 25                y = y + 25                  //  1   1  25   0
mul y x                 y = y * x                   //  1   1   0   0
add y 1                 y = y + 1                   //  1   1   1   0
mul z y                 z = z * y                   //  1   1   1   0
mul y 0                 y = y * 0                   //  1   1   0   0
add y w                 y = y + w                   //  1   1   1   0
add y 6                 y = y + yOffset             //  1   1   7   0
mul y x                 y = y * x                   //  1   1   7   0
add z y                 z = z + y                   //  1   1   7   7


w = n[i]
x = x * 0
x = x + z
x = x % 26
z = z / div
x = x + xOffset
x = x == w ? 1 : 0
x = x == 0 ? 1 : 0

y = y * 0
y = y + 25
y = y * x
y = y + 1
z = z * y

y = y * 0
y = y + w
y = y + yOffset
y = y * x
z = z + y

var z = 0;
for (var i = 0; i < input.Length; i++)
{
    if (xOffset[i] <= 0)
    {
        z = z / 26;
    }

    if ((z % 26 + xOffset[i]) != input[i])
    {
        z = z * 26 + input[i] + yOffset[i]
    }

}


                xOffset     yOffset
input[0]        13          6
input[1]        15          7
input[2]        15          10
input[3]        11          2
input[4]        -7          15
input[5]        10          8
input[6]        10          1
input[7]        -5          10
input[8]        15          5
input[9]        -3          3
input[10]       -0          5
input[11]       -5          11
input[12]       -9          12
input[13]       -0          10

z += input[i] + yOffset[i]
z -= must pop + xOffset[i] == input[i]

z += input[0] + 6
z += input[1] + 7
z += input[2] + 10
z += input[3] + 2
z -= must pop - 7 == input[4]
z += input[5] + 8
z += input[6] + 1
z -= must pop - 5 == input[7]
z += input[8] + 5
z -= must pop - 3 == input[9]
z -= must pop - 0 == input[10]
z -= must pop - 5 == input[11]
z -= must pop - 9 == input[12]
z -= must pop - 0 == input[13]


input[4] = input[3] - 5
input[7] = input[6] - 4
input[9] = input[8] + 2
input[10] = input[5] + 8
input[11] = input[2] + 5
input[12] = input[1] - 2
input[13] = input[0] + 6

              1111
    01234567890123
max(39494195799979)
min(13161151139617)
 */