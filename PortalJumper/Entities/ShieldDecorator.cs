namespace PortalJumper.Entities;

public class ShieldDecorator : IAttackable
{
    private readonly IAttackable _inner;
    public int Hp { get => _inner.Hp; set => _inner.Hp = value; }

    public ShieldDecorator(IAttackable entity) => _inner = entity;

    public void TakeDamage(int damage) => _inner.TakeDamage(0);

    public IAttackable GetInner() => _inner;

    public string GetEffectSymbol() => "🛡️";
}