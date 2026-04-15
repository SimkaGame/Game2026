using PortalJumper.Core;
using PortalJumper.Core.Interfaces;

namespace PortalJumper.Entities;

public class Robot : Monster 
{
    public override string GetSymbol() => "🤖";

    public override void Attack(IAttackable target)
    {
        target.TakeDamage(GameConfig.RobotDamage);
    }

    public override void Move() 
    {
        
    }
}