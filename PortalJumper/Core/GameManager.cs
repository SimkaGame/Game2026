namespace PortalJumper.Core;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using PortalJumper.Entities;
using PortalJumper.Maps;

public class GameManager {
    private static GameManager? instance;
    public static GameManager Instance => instance ??= new GameManager();

    private bool isRunning = true;
    private readonly int MapWidth = 40; 
    private readonly int MapHeight = 15; 
    private readonly Random rng = new Random();

    private Hero hero;
    private IAttackable activeTarget;
    private WorldMap world;
    private readonly List<Monster> monsters = new();
    private readonly List<GoldReward> coins = new();
    
    private Position? shieldPos = null;
    private Position? windPos = null;
    private DateTime shieldExpiry = DateTime.MinValue;
    private DateTime windExpiry = DateTime.MinValue;

    private GameManager() {
        world = new WorldMap();
        world.Initialize(MapWidth, MapHeight);
        hero = new Hero { Position = new Position(7, 15) };
        activeTarget = hero;

        monsters.Add(new Robot { Position = new Position(3, 10) });
        monsters.Add(new TVMonster { Position = new Position(10, 30) });
        coins.Add(new GoldReward(15) { Position = GetRandomEmptyPosition() });
        
        shieldPos = GetRandomEmptyPosition();
        windPos = GetRandomEmptyPosition();
    }

    private Position GetRandomEmptyPosition() {
        while (true) {
            int y = rng.Next(1, MapHeight - 1);
            int x = rng.Next(1, MapWidth - 1);
            Position pos = new Position(y, x);

            if (world.CanMoveTo(pos) && 
                (hero == null || (hero.Position.X != x && hero.Position.Y != y)) &&
                !monsters.Exists(m => m.Position.X == x && m.Position.Y == y)) {
                return pos;
            }
        }
    }

    public void Run() 
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Clear();
        Console.CursorVisible = false;

        while (isRunning) 
        {
            if (Console.KeyAvailable) HandleInput();
            Update();
            Render();
            Thread.Sleep(30);
        }

        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("======================================");
        Console.WriteLine("           ИГРА ОКОНЧЕНА!             ");
        Console.WriteLine($"      Собрано золота: {hero.Gold}     ");
        Console.WriteLine("======================================");
        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    private void HandleInput() {
        var key = Console.ReadKey(true).Key;
        int dy = 0, dx = 0;
        int step = IsActive<WindDecorator>() ? 2 : 1;

        if (key == ConsoleKey.W) dy = -step;
        else if (key == ConsoleKey.S) dy = step;
        else if (key == ConsoleKey.A) dx = -step;
        else if (key == ConsoleKey.D) dx = step;
        else if (key == ConsoleKey.Escape) isRunning = false;

        if (dx == 0 && dy == 0) return;

        Position nextPos = new Position(hero.Position.Y + dy, hero.Position.X + dx);
        if (world.CanMoveTo(nextPos) && !monsters.Exists(m => m.Position.X == nextPos.X && m.Position.Y == nextPos.Y)) {
            hero.Position = nextPos;
        }
    }

    private void Update() {
        if (DateTime.Now > shieldExpiry && IsActive<ShieldDecorator>()) RemoveDecorator<ShieldDecorator>();
        if (DateTime.Now > windExpiry && IsActive<WindDecorator>()) RemoveDecorator<WindDecorator>();
        
        CheckInteractions();

        if (hero.Hp <= 0) isRunning = false;
    }

    private void CheckInteractions() {
        if (shieldPos.HasValue && IsNear(hero.Position, shieldPos.Value)) {
            shieldPos = null;
            shieldExpiry = DateTime.Now.AddSeconds(10);
            activeTarget = new ShieldDecorator(activeTarget); 
        }
        if (windPos.HasValue && IsNear(hero.Position, windPos.Value)) {
            windPos = null;
            windExpiry = DateTime.Now.AddSeconds(10);
            activeTarget = new WindDecorator(activeTarget);
        }

        for (int i = coins.Count - 1; i >= 0; i--) {
            if (IsNear(coins[i].Position, hero.Position)) {
                hero.Gold += coins[i].Amount;
                coins.RemoveAt(i);
            }
        }

        foreach (var m in monsters) {
            if (IsNear(m.Position, hero.Position)) m.Attack(activeTarget);
        }
    }

    private void Render()
    {
        Console.SetCursorPosition(0, 0);
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(new string('#', MapWidth * 2));

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (x == 0 || x == MapWidth - 1)
                {
                    sb.Append("##");
                    continue;
                }

                if (hero.Position.Y == y && hero.Position.X == x)
                {
                    sb.Append("🤠"); 
                }
                else if (monsters.Exists(m => m.Position.Y == y && m.Position.X == x))
                {
                    var m = monsters.Find(mo => mo.Position.Y == y && mo.Position.X == x);
                    sb.Append(m?.GetSymbol() ?? "  "); 
                }
                else if (shieldPos.HasValue && shieldPos.Value.Y == y && shieldPos.Value.X == x)
                {
                    sb.Append("🛡️ "); 
                }
                else if (windPos.HasValue && windPos.Value.Y == y && windPos.Value.X == x)
                {
                    sb.Append("🌬️ "); 
                }
                else if (coins.Exists(c => c.Position.Y == y && c.Position.X == x))
                {
                    sb.Append("💰"); 
                }
                else
                {
                    sb.Append(world.CanMoveTo(new Position(y, x)) ? "  " : "##");
                }
            }
            sb.AppendLine();
        }

        sb.AppendLine(new string('#', MapWidth * 2));
        sb.AppendLine($"HP: {hero.Hp} | Золото: {hero.Gold}".PadRight(MapWidth * 2));
        
        StringBuilder effectsLine = new StringBuilder("Эффекты: ");
        
        if (DateTime.Now < shieldExpiry) 
        {
            int shieldSec = (int)(shieldExpiry - DateTime.Now).TotalSeconds + 1;
            effectsLine.Append($"[Щит 🛡️: {shieldSec}с] ");
        }
        
        if (DateTime.Now < windExpiry) 
        {
            int windSec = (int)(windExpiry - DateTime.Now).TotalSeconds + 1;
            effectsLine.Append($"[Ветер 🌬️: {windSec}с] ");
        }

        if (DateTime.Now >= shieldExpiry && DateTime.Now >= windExpiry)
        {
            effectsLine.Append("нет");
        }

        sb.AppendLine(effectsLine.ToString().PadRight(MapWidth * 2));
        Console.Write(sb.ToString());
    }

    private bool IsNear(Position p1, Position p2) 
    {
        int dx = Math.Abs(p1.X - p2.X);
        int dy = Math.Abs(p1.Y - p2.Y);
        return (dx + dy) <= 1;
    }

    private bool IsActive<T>() {
        var curr = activeTarget;
        while (curr != null) {
            if (curr is T) return true;
            if (curr is ShieldDecorator sd) curr = sd.GetInner();
            else if (curr is WindDecorator wd) curr = wd.GetInner();
            else break;
        }
        return false;
    }

    private void RemoveDecorator<T>() {
        if (activeTarget is T) {
            if (activeTarget is ShieldDecorator sd) activeTarget = sd.GetInner();
            else if (activeTarget is WindDecorator wd) activeTarget = wd.GetInner();
        } else if (activeTarget is ShieldDecorator sd && sd.GetInner() is T) {
            activeTarget = new ShieldDecorator(Unwrap<T>(sd.GetInner()));
        } else if (activeTarget is WindDecorator wd && wd.GetInner() is T) {
            activeTarget = new WindDecorator(Unwrap<T>(wd.GetInner()));
        }
    }

    private IAttackable Unwrap<T>(IAttackable inner) {
        if (inner is ShieldDecorator sd && sd is not T) return sd;
        if (inner is WindDecorator wd && wd is not T) return wd;
        if (inner is ShieldDecorator sd2) return sd2.GetInner();
        if (inner is WindDecorator wd2) return wd2.GetInner();
        return hero;
    }
}