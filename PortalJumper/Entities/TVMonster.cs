namespace PortalJumper.Entities;

public class TVMonster : Monster
{
    public TVMonster()
    {
        Name = "Телевизор";
        Hp = 20;
    }

    public override void Move()
    {
    }

    public override void Attack(Hero hero)
    {
        hero.Hp -= 3;
    }

    public override string GetSymbol()
    {
        return "📺";
    }

    public override object Clone()
    {
        TVMonster clone = new TVMonster();
        clone.Hp = this.Hp;
        return clone;
    }
}