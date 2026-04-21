using PortalJumper.Core;
using PortalJumper.Core.Interfaces;
using PortalJumper.Core.Strategies;

namespace PortalJumper.Entities;

public class TurretAdapter : Monster
{
    public TurretAdapter(Position pos) : base(new PowerAttack())
    {
        Position = pos;
    }

    public override string GetSymbol() => "🛰";
}