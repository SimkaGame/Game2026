namespace PortalJumper.Entities;
using PortalJumper.Core.Interfaces;

public class ExternalLaserTurret {
    public int Damage => 12;

    public void Shoot(object target) {
        if (target is IAttackable attackable) {
            attackable.TakeDamage(Damage);
        }
    }
}