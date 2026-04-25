using System;
using PortalJumper.Core.Interfaces;

namespace PortalJumper.Entities;

public class ShieldDecorator : IAttackable
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

    public ShieldDecorator(IAttackable entity) => _inner = entity;

    public void TakeDamage(int damage)
    {
        _inner.TakeDamage(0);
    }

    public IAttackable GetInner() => _inner;
    public string GetEffectSymbol() => "🛡️";
}