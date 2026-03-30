namespace PortalJumper.Core;

using PortalJumper.Entities;
public class HealthReward : IReward

{
    public int Amount { get; }
    public HealthReward(int amount) => Amount = amount > 0 ? amount : 20;
    public void Collect(Hero hero)
    {
        hero.Hp = Math.Min(hero.Hp + Amount, hero.MaxHp);
    }
}