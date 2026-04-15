using PortalJumper.Core;
using PortalJumper.Core.Interfaces;

namespace PortalJumper.Entities;

public class TVMonster : Monster 
{
    public override string GetSymbol() => "📺";

    public override void Attack(IAttackable target)
    {
        target.TakeDamage(GameConfig.TVMonsterDamage);
    }

    public override void Move()
    {
        
    }
}