using PortalJumper.Core;
using PortalJumper.Core.Interfaces;

namespace PortalJumper.Entities;

public abstract class Monster 
{
    public Position Position { get; set; }
    public abstract string GetSymbol();
    public abstract void Attack(IAttackable target);
    
    public virtual void Move()
    {
        
    }
}