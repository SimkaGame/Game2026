namespace PortalJumper.Entities;

using System;
using PortalJumper.Core;

public class TurretAdapter : Monster {
    private readonly ExternalLaserTurret _externalTurret;

    public TurretAdapter(Position pos) {
        _externalTurret = new ExternalLaserTurret();
        this.Position = pos;
    }

    public override void Attack(IAttackable target) {
        _externalTurret.Shoot(target);
    }

    public override string GetSymbol() => "🛰 ";

    public override void Move() {
    }

    public override Monster Clone() {
        return new TurretAdapter(this.Position);
    }
}