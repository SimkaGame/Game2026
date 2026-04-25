using PortalJumper.Entities;
using Xunit;

namespace PortalJumper.Tests;

public class ObserverTests
{
    [Fact]
    public void Hero_ShouldNotify_WhenHealthChanges()
    {
        var hero = new Hero { Hp = 100 };
        int receivedHp = -1;
        hero.OnHealthChanged += (hp, max) => receivedHp = hp;

        hero.TakeDamage(25);

        Assert.Equal(75, receivedHp);
    }
}