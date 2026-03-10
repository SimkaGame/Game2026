using System.Diagnostics;
using PortalJumper.Entities;
using PortalJumper.Maps;
using PortalJumper.Core.Factories;

namespace PortalJumper.Core;

public class GameManager
{
    private static GameManager? instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();

            return instance;
        }
    }

    private bool isRunning = true;

    private GameWorld world = new();

    private int heroY = 7;
    private int heroX = 20;

    private int gold = 0;

    private List<(int y, int x)> coins = new();
    private List<(int y, int x, Monster monster)> monsters = new();

    private const int TargetFps = 60;
    private const double FrameTime = 1000.0 / TargetFps;

    public void Run()
    {
        Init();

        var stopwatch = new Stopwatch();

        while (isRunning)
        {
            stopwatch.Restart();

            HandleInput();
            Update();
            Render();

            stopwatch.Stop();

            double remaining = FrameTime - stopwatch.Elapsed.TotalMilliseconds;

            if (remaining > 0)
                Thread.Sleep((int)remaining);
        }

        Console.Clear();
        Console.CursorVisible = true;
        Console.WriteLine("Игра окончена");
    }

    private void Init()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        world.Map.Width = 50;
        world.Map.Height = 15;

        Random rand = new();

        for (int i = 0; i < 6; i++)
        {
            int y = rand.Next(1, world.Map.Height - 1);
            int x = rand.Next(1, world.Map.Width - 1);

            coins.Add((y, x));
        }

        var robotFactory = new RobotFactory();
        var tvFactory = new TVFactory();

        monsters.Add((5, 10, robotFactory.CreateMonster()));
        monsters.Add((8, 25, tvFactory.CreateMonster()));
    }

    private void HandleInput()
    {
        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);

            // выход из игры
            if (key.Key == ConsoleKey.Escape)
            {
                isRunning = false;
                return;
            }

            int dy = 0;
            int dx = 0;

            char ch = char.ToLower(key.KeyChar);

            if (ch == 'w') dy = -1;
            if (ch == 's') dy = 1;
            if (ch == 'a') dx = -1;
            if (ch == 'd') dx = 1;

            int newY = heroY + dy;
            int newX = heroX + dx;

            if (newY > 0 && newY < world.Map.Height - 1 &&
                newX > 0 && newX < world.Map.Width - 1)
            {
                heroY = newY;
                heroX = newX;
            }
        }
    }

    private void Update()
    {
        for (int i = coins.Count - 1; i >= 0; i--)
        {
            if (coins[i].y == heroY && coins[i].x == heroX)
            {
                coins.RemoveAt(i);
                gold += 10;
            }
        }

        foreach (var m in monsters)
        {
            if (m.y == heroY && m.x == heroX)
            {
                m.monster.Attack(world.Hero);

                if (world.Hero.Hp <= 0)
                    isRunning = false;
            }
        }
    }

    private void Render()
    {
        for (int y = 0; y < world.Map.Height; y++)
        {
            for (int x = 0; x < world.Map.Width; x++)
            {
                Console.SetCursorPosition(x * 2, y);

                if (y == heroY && x == heroX)
                {
                    Console.Write("🧙");
                    continue;
                }

                var coin = coins.FirstOrDefault(c => c.y == y && c.x == x);

                if (coin != default)
                {
                    Console.Write("💰");
                    continue;
                }

                var monster = monsters.FirstOrDefault(m => m.y == y && m.x == x);

                if (monster.monster != null)
                {
                    Console.Write(monster.monster.GetSymbol());
                    continue;
                }

                if (y == 0 || y == world.Map.Height - 1 ||
                    x == 0 || x == world.Map.Width - 1)
                {
                    Console.Write("##");
                }
                else
                {
                    Console.Write("  ");
                }
            }
        }

        Console.SetCursorPosition(0, world.Map.Height + 1);
        Console.Write($"Золото: {gold}");

        Console.SetCursorPosition(0, world.Map.Height + 2);
        Console.Write($"HP: {world.Hero.Hp}");
    }
}