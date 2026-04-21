namespace PortalJumper.Core.Interfaces;

public interface IAttackStrategy
{
    void ExecuteAttack(IAttackable target);
}