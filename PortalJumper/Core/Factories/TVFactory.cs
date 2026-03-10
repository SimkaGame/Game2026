using PortalJumper.Entities;

namespace PortalJumper.Core.Factories;

public class TVFactory : MonsterFactory
{
    public override Monster CreateMonster()
    {
        return new TVMonster();
    }
}