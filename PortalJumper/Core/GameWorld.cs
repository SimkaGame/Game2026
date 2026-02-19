namespace PortalJumper.Core;

using PortalJumper.Entities;
using PortalJumper.Maps;

public class GameWorld
{
    public Hero Hero { get; set; } = new Hero();        
    public WorldMap Map { get; set; } = new WorldMap();
    public List<Monster> Monsters { get; } = new();
}