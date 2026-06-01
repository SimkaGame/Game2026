using PortalJumper.Core;
using PortalJumper.Core.Interfaces;
using PortalJumper.Maps;
using System;
using System.Collections.Generic;

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

    public virtual void Move(Position targetPos, WorldMap world, List<Monster> allMonsters, Hero hero)
    {
        int deltaY = targetPos.Y - Position.Y;
        int deltaX = targetPos.X - Position.X;

        int stepY = deltaY == 0 ? 0 : (deltaY > 0 ? 1 : -1);
        int stepX = deltaX == 0 ? 0 : (deltaX > 0 ? 1 : -1);

        Position nextPos = Position;

        if (Math.Abs(deltaY) >= Math.Abs(deltaX) && stepY != 0)
        {
            nextPos = new Position(Position.Y + stepY, Position.X);
            if (!IsValidMove(nextPos, world, allMonsters, hero))
            {
                nextPos = new Position(Position.Y, Position.X + stepX);
            }
        }
        else if (stepX != 0)
        {
            nextPos = new Position(Position.Y, Position.X + stepX);
            if (!IsValidMove(nextPos, world, allMonsters, hero))
            {
                nextPos = new Position(Position.Y + stepY, Position.X);
            }
        }

        if (IsValidMove(nextPos, world, allMonsters, hero))
        {
            Position = nextPos;
        }
    }

    private bool IsValidMove(Position pos, WorldMap world, List<Monster> allMonsters, Hero hero)
    {
        if (!world.CanMoveTo(pos)) return false;
        if (hero != null && hero.Position.X == pos.X && hero.Position.Y == pos.Y) return false;
        if (allMonsters.Exists(m => m != this && m.Position.X == pos.X && m.Position.Y == pos.Y)) return false;
        return true;
    }
}