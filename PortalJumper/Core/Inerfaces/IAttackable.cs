namespace PortalJumper.Core.Interfaces;

public interface IAttackable
{
    int Hp { get; set; }
    void TakeDamage(int damage);
}