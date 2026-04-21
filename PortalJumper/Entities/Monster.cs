using PortalJumper.Core;
using PortalJumper.Core.Interfaces;
using PortalJumper.Core.Strategies;

namespace PortalJumper.Entities;

public abstract class Monster 
{
    public Position Position { get; set; }
    protected IAttackStrategy _attackStrategy;

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
        _attackStrategy?.ExecuteAttack(target);
    }

    public virtual void Move()
    {
    }
}