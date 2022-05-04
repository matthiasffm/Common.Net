namespace matthiasffm.Common.Math;

internal class Vec2
{
    public int X { get; set; }
    public int Y { get; set; }

    public Vec2(int x, int y) { X = x; Y = y; }

    public void Add(Vec2 toAdd) { X += toAdd.X; Y += toAdd.Y; }
    public bool In(Vec2 topLeft, Vec2 bottomRight) => X >= topLeft.X && X <= bottomRight.X &&
                                                      Y <= topLeft.Y && Y >= bottomRight.Y;

    public override string ToString() => $"({X}, {Y})";
}
