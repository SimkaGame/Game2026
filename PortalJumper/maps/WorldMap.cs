namespace PortalJumper.Maps;

using System.Collections.Generic;
using PortalJumper.Core;

public class WorldMap
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Cell[,] Cells { get; private set; } = null!;

    public void Initialize(int width, int height)
    {
        Initialize(width, height, 1);
    }

    public void Initialize(int width, int height, int level)
    {
        Width = width;
        Height = height;
        Cells = new Cell[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bool passable = !(y == 0 || y == height - 1 || x == 0 || x == width - 1);

                if (passable && level == 2)
                {
                    if ((x == 15 && y > 3 && y < 11) || (x == 25 && y > 3 && y < 11))
                    {
                        passable = false;
                    }
                }

                if (passable && level == 3)
                {
                    if (y == 5 && x > 5 && x < 35) passable = false;
                    if (y == 10 && x > 5 && x < 35) passable = false;
                    if (x == 20 && (y < 5 || y > 10)) passable = false;
                }

                Cells[y, x] = new Cell(new Position(y, x), passable);
            }
        }
    }

    public Cell GetCell(Position pos)
    {
        if (pos.Y < 0 || pos.Y >= Height || pos.X < 0 || pos.X >= Width)
            return new Cell(pos, false);
        return Cells[pos.Y, pos.X];
    }

    public bool CanMoveTo(Position pos)
    {
        if (pos.Y < 0 || pos.Y >= Height || pos.X < 0 || pos.X >= Width)
            return false;
        return Cells[pos.Y, pos.X].IsPassable;
    }

    public IEnumerable<Cell> GetAllCells()
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                yield return Cells[y, x];
    }
}