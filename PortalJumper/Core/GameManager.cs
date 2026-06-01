namespace PortalJumper.Core;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using PortalJumper.Entities;
using PortalJumper.Maps;
using PortalJumper.Core.Interfaces;
using PortalJumper.Core.States;
using PortalJumper.Core.Commands;
using PortalJumper.Core.DTO;
using PortalJumper.Core.Services;

public class GameManager {
    private static GameManager? instance;
    public static GameManager Instance => instance ??= new GameManager();

    private IGameState _currentState;
    private InputHandler _inputHandler;
    private bool isRunning = true;
    private readonly int MapWidth = 40; 
    private readonly int MapHeight = 15; 
    private readonly Random rng = new Random();

    private Hero hero = null!;
    private ConsoleHUD _hud;
    private IAttackable activeTarget = null!;
    private WorldMap world;
    private readonly List<Monster> monsters = new();
    private readonly List<GoldReward> coins = new();
    private readonly List<FireProjectile> projectiles = new();
    
    private Position? shieldPos = null;
    private Position? windPos = null;
    private DateTime shieldExpiry = DateTime.MinValue;
    private DateTime windExpiry = DateTime.MinValue;

    private int currentLevel = 1;
    private const int MaxLevels = 3;
    private Position? portalPos = null;
    private DateTime lastMonsterMoveTime = DateTime.MinValue;
    private readonly TimeSpan monsterMoveCooldown = TimeSpan.FromMilliseconds(600);
    private DateTime lastProjectileMoveTime = DateTime.MinValue;
    private readonly TimeSpan projectileMoveCooldown = TimeSpan.FromMilliseconds(150);

    private GameManager() {
        world = new WorldMap();
        _hud = new ConsoleHUD();
        _inputHandler = new InputHandler();
        
        SetupDefaultBindings();
        InitLevel();

        _currentState = new GamePlayState(this);
    }

    private void InitLevel() {
        world.Initialize(MapWidth, MapHeight, currentLevel);
        
        hero = new Hero { Position = new Position(7, 2) };
        _hud.Bind(hero);
        activeTarget = hero;

        monsters.Clear();
        coins.Clear();
        projectiles.Clear();

        monsters.Add(new Robot { Position = GetRandomEmptyPosition() });
        monsters.Add(new TVMonster { Position = GetRandomEmptyPosition() });
        
        if (currentLevel >= 2) {
            monsters.Add(new TurretAdapter(GetRandomEmptyPosition()));
        }
        if (currentLevel >= 3) {
            monsters.Add(new Robot { Position = GetRandomEmptyPosition() });
            monsters.Add(new TVMonster { Position = GetRandomEmptyPosition() });
        }
        
        coins.Add(new GoldReward(15) { Position = GetRandomEmptyPosition() });
        shieldPos = GetRandomEmptyPosition();
        windPos = GetRandomEmptyPosition();
        portalPos = GetRandomEmptyPosition();
    }

    private void SetupDefaultBindings() {
        _inputHandler.Bind(ConsoleKey.W, new MoveCommand(hero, -1, 0));
        _inputHandler.Bind(ConsoleKey.S, new MoveCommand(hero, 1, 0));
        _inputHandler.Bind(ConsoleKey.A, new MoveCommand(hero, 0, -1));
        _inputHandler.Bind(ConsoleKey.D, new MoveCommand(hero, 0, 1));
        _inputHandler.Bind(ConsoleKey.K, new SaveGameCommand(this));
        _inputHandler.Bind(ConsoleKey.L, new LoadGameCommand(this));
    }

    public void NextLevel() {
        if (currentLevel >= MaxLevels) {
            ShowVictoryScreen();
            return;
        }
        
        currentLevel++;
        InitLevel();
        
        Console.Clear();
        Console.WriteLine($"\n\n   ПЕРЕХОД НА УРОВЕНЬ {currentLevel}! ");
        Thread.Sleep(1500);
    }

    private void ShowVictoryScreen() {
        while (Console.KeyAvailable) {
            Console.ReadKey(true);
        }
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("           ПОЗДРАВЛЯЕМ!               ");
        Console.WriteLine("        ВЫ ПРОШЛИ ВСЮ ИГРУ!           ");
        Console.WriteLine($"      Финальное золото: {hero.Gold}   ");
        Console.WriteLine("======================================");
        Console.WriteLine(" Нажмите любую клавишу для выхода...  ");
        Console.ReadKey(true);
        isRunning = false;
    }

    public void SaveGame() {
        var data = new SaveData {
            HeroHp = hero.Hp,
            HeroMaxHp = hero.MaxHp,
            HeroGold = hero.Gold,
            HeroX = hero.Position.X,
            HeroY = hero.Position.Y,
            CurrentLevel = currentLevel,
            ShieldPos = shieldPos.HasValue ? new PositionDto { X = shieldPos.Value.X, Y = shieldPos.Value.Y } : null,
            WindPos = windPos.HasValue ? new PositionDto { X = windPos.Value.X, Y = windPos.Value.Y } : null,
            PortalPos = portalPos.HasValue ? new PositionDto { X = portalPos.Value.X, Y = portalPos.Value.Y } : null
        };

        foreach (var m in monsters) {
            data.Monsters.Add(new MonsterSaveDto {
                Type = m.GetType().Name,
                X = m.Position.X,
                Y = m.Position.Y
            });
        }

        foreach (var c in coins) {
            data.Coins.Add(new PositionDto { X = c.Position.X, Y = c.Position.Y });
        }

        SaveService.Save(data);
    }

    public void LoadGame() {
        var data = SaveService.Load();
        if (data != null) {
            currentLevel = data.CurrentLevel;
            world.Initialize(MapWidth, MapHeight, currentLevel);
            
            hero = new Hero { Position = new Position(data.HeroY, data.HeroX) };
            hero.MaxHp = data.HeroMaxHp;
            hero.Hp = data.HeroHp;
            hero.Gold = data.HeroGold;
            _hud.Bind(hero);
            activeTarget = hero;

            monsters.Clear();
            coins.Clear();
            projectiles.Clear();

            foreach (var mDto in data.Monsters) {
                Monster monster = mDto.Type switch {
                    "Robot" => new Robot { Position = new Position(mDto.Y, mDto.X) },
                    "TVMonster" => new TVMonster { Position = new Position(mDto.Y, mDto.X) },
                    "TurretAdapter" => new TurretAdapter(new Position(mDto.Y, mDto.X)),
                    _ => null!
                };
                if (monster != null) monsters.Add(monster);
            }

            foreach (var cDto in data.Coins) {
                coins.Add(new GoldReward(15) { Position = new Position(cDto.Y, cDto.X) });
            }

            shieldPos = data.ShieldPos != null ? new Position(data.ShieldPos.Y, data.ShieldPos.X) : null;
            windPos = data.WindPos != null ? new Position(data.WindPos.Y, data.WindPos.X) : null;
            portalPos = data.PortalPos != null ? new Position(data.PortalPos.Y, data.PortalPos.X) : null;
            
            Console.Clear();
        }
    }

    public void RemapKey(ConsoleKey key, ICommand newCommand) => _inputHandler.Bind(key, newCommand);
    public void SetState(IGameState newState) => _currentState = newState;
    public int GetHeroHp() => hero.Hp;
    public int GetHeroGold() => hero.Gold;

    public void Run() {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Clear();
        ShowStartScreen();
        while (isRunning) {
            if (Console.KeyAvailable) _currentState.HandleInput();
            _currentState.Update();
            _currentState.Render();
            Thread.Sleep(30);
        }
    }

    private void ShowStartScreen() {
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("        ДОБРО ПОЖАЛОВАТЬ В            ");
        Console.WriteLine("          PORTAL JUMPER!              ");
        Console.WriteLine("======================================");
        Console.WriteLine(" Управление: WASD                     ");
        Console.WriteLine(" Сохранение: K  |  Загрузка: L        ");
        Console.WriteLine("======================================");
        Console.WriteLine(" Нажмите ЛЮБУЮ КЛАВИШУ для старта...");
        Console.ReadKey(true);
        Console.Clear();
    }

    public void ProcessHeroInput(ConsoleKey key) => _inputHandler.HandleInput(key);

    public void MoveHero(int dy, int dx) {
        int step = IsActive<SpeedDecorator>() ? 2 : 1;
        Position nextPos = new Position(hero.Position.Y + (dy * step), hero.Position.X + (dx * step));
        
        if (world.CanMoveTo(nextPos) && !monsters.Exists(m => m.Position.X == nextPos.X && m.Position.Y == nextPos.Y)) {
            hero.Position = nextPos;
        }
    }

    public void UpdateGameLogic() {
        if (DateTime.Now > shieldExpiry && IsActive<ShieldDecorator>()) RemoveDecorator<ShieldDecorator>();
        if (DateTime.Now > windExpiry && IsActive<SpeedDecorator>()) RemoveDecorator<SpeedDecorator>();
        
        foreach (var m in monsters) {
            if (m is TurretAdapter turret) {
                turret.CheckShooting(projectiles);
            }
        }

        if (DateTime.Now - lastProjectileMoveTime >= projectileMoveCooldown) {
            for (int i = projectiles.Count - 1; i >= 0; i--) {
                projectiles[i].Update(world, hero);
                if (projectiles[i].IsDestroyed) {
                    projectiles.RemoveAt(i);
                }
            }
            lastProjectileMoveTime = DateTime.Now;
        }

        if (DateTime.Now - lastMonsterMoveTime >= monsterMoveCooldown) {
            foreach (var m in monsters) {
                m.Move(hero.Position, world, monsters, hero);
            }
            lastMonsterMoveTime = DateTime.Now;
        }

        CheckInteractions();
        if (hero.Hp <= 0) SetState(new GameOverState(this));
    }

    private void CheckInteractions() {
        if (portalPos.HasValue && hero.Position.X == portalPos.Value.X && hero.Position.Y == portalPos.Value.Y) {
            NextLevel();
            return;
        }

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
        foreach (var m in monsters) if (IsNear(m.Position, hero.Position)) m.Attack(activeTarget);
    }

    public void RenderGame() {
        Console.SetCursorPosition(0, 0);
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < MapHeight; y++) {
            for (int x = 0; x < MapWidth; x++) sb.Append(GetSymbolAt(x, y));
            sb.AppendLine();
        }
        RenderHUD(sb);
        Console.Write(sb.ToString());
    }

    public void ShowGameOverScreen() {
        while (Console.KeyAvailable) {
            Console.ReadKey(true);
        }
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("           ИГРА ОКОНЧЕНА!             ");
        Console.WriteLine($"      Собрано золота: {hero.Gold}     ");
        Console.WriteLine("======================================");
        Console.WriteLine(" Нажмите любую клавишу для выхода...  ");
        Console.ReadKey(true);
        isRunning = false;
    }

    private string GetSymbolAt(int x, int y) {
        if (hero.Position.Y == y && hero.Position.X == x) return "🏃";
        if (portalPos.HasValue && portalPos.Value.Y == y && portalPos.Value.X == x) return "🌀";

        var m = monsters.Find(mo => mo.Position.Y == y && mo.Position.X == x);
        if (m != null) {
            string s = m.GetSymbol();
            if (s == "🛰") return s + " "; 
            return s.Length > 1 ? s : s + " ";
        }

        var p = projectiles.Find(pr => pr.Position.Y == y && pr.Position.X == x);
        if (p != null) return "🔥";

        if (shieldPos.HasValue && shieldPos.Value.Y == y && shieldPos.Value.X == x) return "🛡️ ";
        if (windPos.HasValue && windPos.Value.Y == y && windPos.Value.X == x) return "🌬️ ";
        if (coins.Exists(c => c.Position.Y == y && c.Position.X == x)) return "💰";
        return world.CanMoveTo(new Position(y, x)) ? "  " : "##";
    }

    private void RenderHUD(StringBuilder sb) {
        sb.AppendLine($"Уровень: {currentLevel}/{MaxLevels} | HP: {hero.Hp} | Золото: {hero.Gold}".PadRight(MapWidth * 2));
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
            if (world.CanMoveTo(pos) && !monsters.Exists(m => m.Position.X == x && m.Position.Y == y) && !(hero?.Position.X == x && hero?.Position.Y == y)) return pos;
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
        }
    }
}