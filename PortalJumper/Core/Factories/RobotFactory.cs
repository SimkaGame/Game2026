using PortalJumper.Entities;

namespace PortalJumper.Core.Factories;

public class RobotFactory : MonsterFactory
{
    public override Monster CreateMonster()
    {
        return new Robot();
    }
}