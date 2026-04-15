using PortalJumper.Core;
using PortalJumper.Core.Interfaces;

namespace PortalJumper.Entities;

public class TurretAdapter : Monster
{
    public TurretAdapter(Position pos)
    {
        Position = pos;
    }

    public override string GetSymbol() => "🛰";

    public override void Attack(IAttackable target)
    {
        target.TakeDamage(GameConfig.TurretDamage);
    }
}