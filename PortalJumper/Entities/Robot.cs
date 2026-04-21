using PortalJumper.Core.Interfaces;
using PortalJumper.Core.Strategies;

namespace PortalJumper.Entities;

public class Robot : Monster 
{
    public Robot() : base(new SimpleAttack()) 
    {
    }

    public override string GetSymbol() => "🤖";
}