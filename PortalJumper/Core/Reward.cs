namespace PortalJumper.Core;
using PortalJumper.Entities;

public interface IReward
{
    void Collect(Hero hero);
}

public class GoldReward : IReward
{
    public int Amount { get; }

    public GoldReward(int amount) => Amount = amount > 0 ? amount : 10;

    public void Collect(Hero hero)
    {
        Console.WriteLine($"Получено {Amount} золота!");
    }
}

public class HealthReward : IReward
{
    public int Amount { get; }

    public HealthReward(int amount) => Amount = amount > 0 ? amount : 20;

    public void Collect(Hero hero)
    {
        hero.Hp = Math.Min(hero.Hp + Amount, hero.MaxHp);
        Console.WriteLine($"Восстановлено {Amount} здоровья!");
    }
}