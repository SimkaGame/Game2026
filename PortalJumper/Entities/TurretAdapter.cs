using PortalJumper.Core;
using PortalJumper.Core.Interfaces;
using PortalJumper.Core.Strategies;
using PortalJumper.Maps;
using System;
using System.Collections.Generic;

namespace PortalJumper.Entities;

public class TurretAdapter : Monster
{
    private DateTime _lastShootTime = DateTime.MinValue;
    private readonly TimeSpan _shootCooldown = TimeSpan.FromSeconds(2.0);

    public TurretAdapter(Position pos) : base(new PowerAttack())
    {
        Position = pos;
    }

    public override string GetSymbol() => "🛰";

    public override void Move(Position targetPos, WorldMap world, List<Monster> allMonsters, Hero hero)
    {
    }

    public void CheckShooting(List<FireProjectile> projectiles)
    {
        if (DateTime.Now - _lastShootTime >= _shootCooldown)
        {
            projectiles.Add(new FireProjectile(Position, -1, 0));
            projectiles.Add(new FireProjectile(Position, 1, 0));
            projectiles.Add(new FireProjectile(Position, 0, -1));
            projectiles.Add(new FireProjectile(Position, 0, 1));
            _lastShootTime = DateTime.Now;
        }
    }
}