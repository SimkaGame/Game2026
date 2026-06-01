using System;
using PortalJumper.Core.Interfaces;
using PortalJumper.Core;

namespace PortalJumper.Entities;

public class Hero : IAttackable
{
    private int _hp = 100;
    public int MaxHp { get; set; } = 100;
    public int Gold { get; set; } = 0;
    public Position Position { get; set; }
    
    public event Action<int, int>? OnHealthChanged;

    public int Hp
    {
        get => _hp;
        set
        {
            if (_hp == value) return;
            _hp = value;
            OnHealthChanged?.Invoke(_hp, MaxHp);
        }
    }

    public string GetSymbol() => "🏃";

    public void TakeDamage(int damage)
    {
        Hp -= damage;
    }
}