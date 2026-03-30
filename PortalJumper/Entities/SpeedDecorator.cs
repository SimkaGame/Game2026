namespace PortalJumper.Entities;

public class WindDecorator : IAttackable
{
    private readonly IAttackable _inner;
    public int Hp { get => _inner.Hp; set => _inner.Hp = value; }

    public WindDecorator(IAttackable entity) => _inner = entity;

    public void TakeDamage(int damage) => _inner.TakeDamage(damage);

    public IAttackable GetInner() => _inner;

    public string GetEffectSymbol() => "🌬️";
}