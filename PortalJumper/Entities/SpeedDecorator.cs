using System;
using PortalJumper.Core.Interfaces;

namespace PortalJumper.Entities;

public class SpeedDecorator : IAttackable
{
    private readonly IAttackable _inner;

    public int Hp 
    { 
        get => _inner.Hp; 
        set => _inner.Hp = value; 
    }

    public int MaxHp 
    { 
        get => _inner.MaxHp; 
        set => _inner.MaxHp = value; 
    }

    public event Action<int, int> OnHealthChanged
    {
        add => _inner.OnHealthChanged += value;
        remove => _inner.OnHealthChanged -= value;
    }

    public SpeedDecorator(IAttackable entity) => _inner = entity;

    public void TakeDamage(int damage) => _inner.TakeDamage(damage);

    public IAttackable GetInner() => _inner;

    public string GetEffectSymbol() => "🌬️";
}