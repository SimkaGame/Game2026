using PortalJumper.Core.Interfaces;

namespace PortalJumper.Core.Strategies;

public class SimpleAttack : IAttackStrategy
{
    public void ExecuteAttack(IAttackable target)
    {
        target.TakeDamage(5);
    }
}