using PortalJumper.Core;
using PortalJumper.Core.Interfaces;

namespace PortalJumper.Entities;

public class Hero : IAttackable
{
    public int Hp { get; set; } = 100;
    public int MaxHp { get; set; } = 100;
    public int Gold { get; set; } = 0;
    public Position Position { get; set; }
    public string GetSymbol() => "🤠";

    public void TakeDamage(int damage)
    {
        Hp -= damage;
    }
}