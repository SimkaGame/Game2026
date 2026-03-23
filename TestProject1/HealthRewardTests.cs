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
    
    [Fact]
    public void Collect_ShouldNotExceedMaxHp()
    {
        var hero = new Hero { Hp = 90, MaxHp = 100 };
        var reward = new HealthReward(20);

        reward.Collect(hero);

        Assert.Equal(100, hero.Hp);
    }

    [Fact]
    public void Collect_WithZeroHp_ShouldHeal()
    {
        var hero = new Hero { Hp = 0, MaxHp = 100 };
        var reward = new HealthReward(30);

        reward.Collect(hero);

        Assert.Equal(30, hero.Hp);
    }

    [Fact]
    public void Collect_WithHugeAmount_ShouldCapAtMaxHp()
    {
        var hero = new Hero { Hp = 10, MaxHp = 100 };
        var reward = new HealthReward(999);

        reward.Collect(hero);

        Assert.Equal(100, hero.Hp);
    }

    [Fact]
    public void Constructor_WithNegativeAmount_ShouldUseDefault()
    {
        var reward = new HealthReward(-10);

        var amount = reward.Amount;

        Assert.Equal(20, amount);
    }
}