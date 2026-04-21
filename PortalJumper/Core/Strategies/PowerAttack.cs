using PortalJumper.Core.Interfaces;

namespace PortalJumper.Core.Strategies;

public class PowerAttack : IAttackStrategy
{
    public void ExecuteAttack(IAttackable target)
    {
        target.TakeDamage(15);
    }
}