namespace PortalJumper.Entities;
using PortalJumper.Items;

public class Hero
{
    public string Name { get; set; } = "Безымянный герой";

    public Inventory Inventory { get; set; } = new Inventory();

    public void Move() { }
}