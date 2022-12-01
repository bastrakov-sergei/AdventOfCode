using System.Drawing;
using System.Drawing.Imaging;

namespace Day20;

public static class Extensions
{
    public static int ToColor(this char c)
        => c switch
        {
            '#' => 1,
            '.' => 0,
            _ => throw new ArgumentOutOfRangeException()
        };

    public static void Render(this Image image, string name)
    {
        var bitmap = new Bitmap(image.Witdh, image.Height, PixelFormat.Format32bppRgb);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.FillRectangle(Brushes.Black, 0, 0, image.Witdh, image.Height);
            foreach (var pixel in image.Pixels)
            {
                g.FillRectangle(Brushes.White, pixel.X, pixel.Y, 1, 1);
            }
        }

        bitmap.Save($"{name}.png", ImageFormat.Png);
    }
}