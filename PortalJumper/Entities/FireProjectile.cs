using PortalJumper.Core;
using PortalJumper.Maps;

namespace PortalJumper.Entities;

public class FireProjectile
{
    public Position Position { get; private set; }
    private readonly int _dy;
    private readonly int _dx;
    public bool IsDestroyed { get; private set; }

    public FireProjectile(Position startPos, int dy, int dx)
    {
        Position = startPos;
        _dy = dy;
        _dx = dx;
    }

    public void Update(WorldMap world, Hero hero)
    {
        if (IsDestroyed) return;

        Position nextPos = new Position(Position.Y + _dy, Position.X + _dx);

        if (!world.CanMoveTo(nextPos))
        {
            IsDestroyed = true;
            return;
        }

        Position = nextPos;

        if (hero.Position.X == Position.X && hero.Position.Y == Position.Y)
        {
            hero.TakeDamage(15);
            IsDestroyed = true;
        }
    }
}