using PortalJumper.Core;

namespace PortalJumper.Entities;

public class TVMonster : Monster
{
    public TVMonster() { Name = "Телевизор"; Hp = 20; }

    public override void Move() { }
    public override void Attack(IAttackable target) => target.TakeDamage(3);
    
    public override string GetSymbol() => "📺";

    public override object Clone()
    {
        return new TVMonster { Hp = this.Hp, Position = this.Position };
    }
}