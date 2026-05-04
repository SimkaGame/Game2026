namespace PortalJumper.Core;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using PortalJumper.Entities;
using PortalJumper.Maps;
using PortalJumper.Core.Interfaces;
using PortalJumper.Core.States;

public class GameManager {
    private static GameManager? instance;
    public static GameManager Instance => instance ??= new GameManager();

    private IGameState _currentState;
    private bool isRunning = true;
    private readonly int MapWidth = 40; 
    private readonly int MapHeight = 15; 
    private readonly Random rng = new Random();

    private Hero hero;
    private ConsoleHUD _hud;
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
        
        _hud = new ConsoleHUD();
        _hud.Bind(hero);

        activeTarget = hero;

        monsters.Add(new Robot { Position = new Position(3, 10) });
        monsters.Add(new TVMonster { Position = new Position(10, 30) });
        monsters.Add(new TurretAdapter(new Position(5, 20)));
        
        coins.Add(new GoldReward(15) { Position = GetRandomEmptyPosition() });
        shieldPos = GetRandomEmptyPosition();
        windPos = GetRandomEmptyPosition();

        _currentState = new GamePlayState(this);
    }

    public void SetState(IGameState newState) => _currentState = newState;
    
    public int GetHeroHp() => hero.Hp;
    
    public int GetHeroGold() => hero.Gold;

    public void Run() {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Clear();
        Console.CursorVisible = false;
        
        while (isRunning) {
            if (Console.KeyAvailable) _currentState.HandleInput();
            _currentState.Update();
            _currentState.Render();
            Thread.Sleep(30);
        }
    }

    public void ProcessHeroInput(ConsoleKey key) {
        int dy = 0, dx = 0;
        int step = IsActive<SpeedDecorator>() ? 2 : 1;
        
        if (key == ConsoleKey.W) dy = -step;
        else if (key == ConsoleKey.S) dy = step;
        else if (key == ConsoleKey.A) dx = -step;
        else if (key == ConsoleKey.D) dx = step;
        
        if (dx == 0 && dy == 0) return;
        
        Position nextPos = new Position(hero.Position.Y + dy, hero.Position.X + dx);
        if (world.CanMoveTo(nextPos) && !monsters.Exists(m => m.Position.X == nextPos.X && m.Position.Y == nextPos.Y)) {
            hero.Position = nextPos;
        }
    }

    public void UpdateGameLogic() {
        if (DateTime.Now > shieldExpiry && IsActive<ShieldDecorator>()) RemoveDecorator<ShieldDecorator>();
        if (DateTime.Now > windExpiry && IsActive<SpeedDecorator>()) RemoveDecorator<SpeedDecorator>();
        
        CheckInteractions();
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
            activeTarget = new SpeedDecorator(activeTarget);
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

    public void RenderGame() {
        Console.SetCursorPosition(0, 0);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(new string('#', MapWidth * 2));
        for (int y = 0; y < MapHeight; y++) {
            for (int x = 0; x < MapWidth; x++) {
                sb.Append(GetSymbolAt(x, y));
            }
            sb.AppendLine();
        }
        sb.AppendLine(new string('#', MapWidth * 2));
        RenderHUD(sb);
        Console.Write(sb.ToString());
    }

    public void ShowGameOverScreen() {
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("           ИГРА ОКОНЧЕНА!             ");
        Console.WriteLine($"      Собрано золота: {hero.Gold}     ");
        Console.WriteLine("======================================");
        Console.WriteLine("Нажмите любую клавишу для выхода...");
        isRunning = false;
    }

    private string GetSymbolAt(int x, int y) {
        if (x == 0 || x == MapWidth - 1) return "##";
        
        if (hero.Position.Y == y && hero.Position.X == x) return "🤠";
        
        var m = monsters.Find(mo => mo.Position.Y == y && mo.Position.X == x);
        if (m != null) {
            string s = m.GetSymbol();
            if (s == "🛰") return s + " "; 
            return s.Length > 1 ? s : s + " ";
        }

        if (shieldPos.HasValue && shieldPos.Value.Y == y && shieldPos.Value.X == x) return "🛡️ ";
        if (windPos.HasValue && windPos.Value.Y == y && windPos.Value.X == x) return "🌬️ ";
        if (coins.Exists(c => c.Position.Y == y && c.Position.X == x)) return "💰";

        return world.CanMoveTo(new Position(y, x)) ? "  " : "##";
    }

    private void RenderHUD(StringBuilder sb) {
        sb.AppendLine($"HP: {hero.Hp} | Золото: {hero.Gold}".PadRight(MapWidth * 2));
        StringBuilder effectsLine = new StringBuilder("Эффекты: ");
        if (DateTime.Now < shieldExpiry) effectsLine.Append($"[Щит 🛡️: {(int)(shieldExpiry - DateTime.Now).TotalSeconds + 1}с] ");
        if (DateTime.Now < windExpiry) effectsLine.Append($"[Ветер 🌬️: {(int)(windExpiry - DateTime.Now).TotalSeconds + 1}с] ");
        if (DateTime.Now >= shieldExpiry && DateTime.Now >= windExpiry) effectsLine.Append("нет");
        sb.AppendLine(effectsLine.ToString().PadRight(MapWidth * 2));
    }

    private Position GetRandomEmptyPosition() {
        while (true) {
            int y = rng.Next(1, MapHeight - 1);
            int x = rng.Next(1, MapWidth - 1);
            Position pos = new Position(y, x);
            if (world.CanMoveTo(pos) && (hero == null || (hero.Position.X != x && hero.Position.Y != y)) && !monsters.Exists(m => m.Position.X == x && m.Position.Y == y)) {
                return pos;
            }
        }
    }

    private bool IsNear(Position p1, Position p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) <= 1;

    private bool IsActive<T>() {
        var curr = activeTarget;
        while (curr != null) {
            if (curr is T) return true;
            if (curr is ShieldDecorator sd) curr = sd.GetInner();
            else if (curr is SpeedDecorator wd) curr = wd.GetInner();
            else break;
        }
        return false;
    }

    private void RemoveDecorator<T>() {
        if (activeTarget is T) {
            if (activeTarget is ShieldDecorator sd) activeTarget = sd.GetInner();
            else if (activeTarget is SpeedDecorator wd) activeTarget = wd.GetInner();
        } else if (activeTarget is ShieldDecorator sd && sd.GetInner() is T) {
            activeTarget = new ShieldDecorator(Unwrap<T>(sd.GetInner()));
        } else if (activeTarget is SpeedDecorator wd && wd.GetInner() is T) {
            activeTarget = new SpeedDecorator(Unwrap<T>(wd.GetInner()));
        }
    }

    private IAttackable Unwrap<T>(IAttackable inner) {
        if (inner is ShieldDecorator sd && sd is not T) return sd;
        if (inner is SpeedDecorator wd && wd is not T) return wd;
        return hero;
    }
}