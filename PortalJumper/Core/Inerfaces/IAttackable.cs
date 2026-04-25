using System;

namespace PortalJumper.Core.Interfaces;

public interface IAttackable
{
    int Hp { get; set; }
    int MaxHp { get; set; }
    event Action<int, int> OnHealthChanged;
    void TakeDamage(int damage);
}