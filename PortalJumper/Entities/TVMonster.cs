using PortalJumper.Core.Interfaces;
using PortalJumper.Core.Strategies;

namespace PortalJumper.Entities;

public class TVMonster : Monster 
{
    public TVMonster() : base(new SimpleAttack())
    {
    }

    public override string GetSymbol() => "📺";
}