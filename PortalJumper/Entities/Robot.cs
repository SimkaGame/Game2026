using PortalJumper.Core;

namespace PortalJumper.Entities;

public class Robot : Monster
{
    public Robot() { Name = "Робот"; Hp = 25; }

    public override void Move() { }
    public override void Attack(IAttackable target) => target.TakeDamage(5);
    
    public override string GetSymbol() => "🤖"; 

    public override object Clone()
    {
        return new Robot { Hp = this.Hp, Position = this.Position };
    }
}