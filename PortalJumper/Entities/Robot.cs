namespace PortalJumper.Entities;

public class Robot : Monster
{
    public Robot()
    {
        Name = "Робот";
        Hp = 25;
    }

    public override void Move()
    {
    }

    public override void Attack(Hero hero)
    {
        hero.Hp -= 5;
    }

    public override string GetSymbol()
    {
        return "🤖";
    }

    public override object Clone()
    {
        Robot clone = new Robot();
        clone.Hp = this.Hp;
        return clone;
    }
}