namespace PortalJumper.Core;

public readonly struct Position
{
    public int Y { get; init; }
    public int X { get; init; }

    public Position(int y, int x) => (Y, X) = (y, x);

    public Position UpNeighbor()    => this with { Y = Y - 1 };
    public Position DownNeighbor()  => this with { Y = Y + 1 };
    public Position LeftNeighbor()  => this with { X = X - 1 };
    public Position RightNeighbor() => this with { X = X + 1 };
}