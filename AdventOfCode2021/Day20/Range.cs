using System.Collections;

namespace Day20;

public record Range(Point TopLeft, Point BottomRight) : IEnumerable<Point>
{
    public int Left => TopLeft.X;
    public int Right => BottomRight.X;
    public int Top => TopLeft.Y;
    public int Bottom => BottomRight.Y;
    public int Width => Right - Left;
    public int Height => Bottom - Top;

    public Range Enlarge(int value)
        => new(TopLeft - Point.One * value, BottomRight + Point.One * value);

    public bool Contains(Point point)
        => Left <= point.X && point.X < Right &&
           Top <= point.Y && point.Y < Bottom;

    public IEnumerator<Point> GetEnumerator()
        => new PointEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class PointEnumerator : IEnumerator<Point>
    {
        private readonly Range range;
        private int currentX;
        private int currentY;

        public PointEnumerator(Range range)
        {
            this.range = range;
            currentX = range.Left - 1;
            currentY = range.Top;
        }

        public bool MoveNext()
        {
            if (currentX + 1 < range.Right)
            {
                currentX += 1;
                return true;
            }

            if (currentY + 1 < range.Bottom)
            {
                currentY += 1;
                currentX = range.Left;
                return true;
            }

            return false;
        }

        public void Reset()
            => (currentX, currentY) = (range.Left - 1, range.Top);

        public Point Current => new(currentX, currentY);

        object IEnumerator.Current
            => Current;

        public void Dispose()
            => Reset();
    }
}