namespace PortalJumper.Core.Factories;
using PortalJumper.Entities;

public abstract class MonsterFactory
{
    public abstract Monster CreateMonster();

    public Monster Spawn()
    {
        return CreateMonster();
    }
}