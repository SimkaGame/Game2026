using PortalJumper.Core; // Для доступа к классу Position

namespace PortalJumper.Entities;

// Интерфейс остается здесь, раз тебе так удобнее
public interface IAttackable
{
    int Hp { get; set; }
    void TakeDamage(int damage);
}

public class Hero : IAttackable
{
    public int Hp { get; set; } = 100;
    public int MaxHp { get; set; } = 100;
    public int Gold { get; set; } = 0;
    public Position Position { get; set; }

    // Добавляем метод, чтобы GM не хардкодил эмодзи
    public string GetSymbol() => "🤠";

    public void TakeDamage(int damage)
    {
        Hp -= damage;
    }
}