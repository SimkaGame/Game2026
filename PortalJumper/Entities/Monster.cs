using PortalJumper.Items;

namespace PortalJumper.Entities;

public abstract class Monster
{
    public string Name { get; protected set; }
    public int Hp { get; protected set; } = 10;

    public Inventory Inventory { get; set; } = new Inventory();

    public abstract void Move();

    public abstract void Attack(Hero hero);

    public abstract string GetSymbol();
}