namespace PortalJumper.Entities;
using PortalJumper.Items;

public abstract class Monster
{
    public string Name { get; set; } = "Монстр";

    public int Hp { get; set; } = 10;

    public Inventory Inventory { get; set; } = new Inventory();

    public abstract void Move();
    public abstract void Attack();
}