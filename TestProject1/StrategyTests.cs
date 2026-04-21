using PortalJumper.Entities;
using PortalJumper.Core.Strategies;
using Xunit;

namespace PortalJumper.Tests;

public class StrategyTests
{
    [Fact]
    public void Monster_ShouldChangeDamage_WhenStrategySwitched()
    {
        var hero = new Hero { Hp = 100 };
        var robot = new Robot();

        robot.SetStrategy(new PowerAttack());
        robot.Attack(hero);
        int hpAfterPower = hero.Hp;

        robot.SetStrategy(new IdleStrategy());
        robot.Attack(hero);

        Assert.Equal(85, hpAfterPower);
        Assert.Equal(85, hero.Hp);
    }
}