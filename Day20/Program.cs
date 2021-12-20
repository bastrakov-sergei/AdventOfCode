using Day20;
using Range = Day20.Range;

var input = File.ReadAllLines("example.txt");

var enhanceMap = input[0]
    .Select(c => c.ToColor())
    .ToArray();

var pixels = input
    .Skip(2)
    .SelectMany((line, y) => line.Select((pixel, x) => (Point: new Point(x, y), Color: pixel.ToColor())))
    .ToArray();

var range = new Range(Point.Zero,
    new Point(
        pixels.Select(p => p.Point.X).Max() + 1,
        pixels.Select(p => p.Point.X).Max() + 1)
);

var image = new Image(
    range,
    pixels
        .Where(p => p.Color == 1)
        .Select(p => p.Point)
        .ToHashSet(),
    enhanceMap,
    0);
image.Render("image");
image = image.Enhance();
image.Render("image0");
image = image.Enhance();
image.Render("image1");
Console.WriteLine($"Part 1: {image.Pixels.Count}");

for (var i = 0; i < 48; i++)
{
    image = image.Enhance();
    image.Render($"image{i+2}");
}

Console.WriteLine($"Part 2: {image.Pixels.Count}");