using PortalJumper.Items;

namespace PortalJumper.Entities;

public class Hero
{
    public string Name { get; set; }
    public int Hp { get; set; } = 100;
    public int MaxHp { get; set; } = 100;
    public Inventory Inventory { get; set; } = new Inventory();
}