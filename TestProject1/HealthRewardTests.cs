using PortalJumper.Core;
using PortalJumper.Entities;
using Xunit;

public class HealthRewardTests
{
    [Fact]
    public void Collect_ShouldIncreaseHp()
    {
        var hero = new Hero { Hp = 50, MaxHp = 100 };
        var reward = new HealthReward(20);

        reward.Collect(hero);

        Assert.Equal(70, hero.Hp);
    }
}