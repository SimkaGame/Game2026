using PortalJumper.Core;
using System;

namespace PortalJumper.Entities;

public abstract class Monster : ICloneable
{
    public string Name { get; protected set; } = "";
    public int Hp { get; set; } = 10;
    public Position Position { get; set; } = new Position(0, 0);

    public abstract void Move();
    public abstract void Attack(IAttackable target);
    
    public abstract string GetSymbol(); 
    
    public abstract object Clone();
}