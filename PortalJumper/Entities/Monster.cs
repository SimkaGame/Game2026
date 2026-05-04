using PortalJumper.Core;
using PortalJumper.Core.Interfaces;
using System;

namespace PortalJumper.Entities;

public abstract class Monster 
{
    public Position Position { get; set; }
    protected IAttackStrategy _attackStrategy;
    
    private DateTime _lastAttackTime = DateTime.MinValue;
    private readonly TimeSpan _attackCooldown = TimeSpan.FromSeconds(1.0);

    public Monster(IAttackStrategy defaultStrategy)
    {
        _attackStrategy = defaultStrategy;
    }

    public abstract string GetSymbol();
    
    public void SetStrategy(IAttackStrategy strategy)
    {
        _attackStrategy = strategy;
    }

    public void Attack(IAttackable target)
    {
        if (DateTime.Now - _lastAttackTime < _attackCooldown)
        {
            return;
        }

        _attackStrategy?.ExecuteAttack(target);
        _lastAttackTime = DateTime.Now;
    }

    public virtual void Move()
    {
    }
}