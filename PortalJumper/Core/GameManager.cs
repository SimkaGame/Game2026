using System.Diagnostics;
using PortalJumper.Entities;
using PortalJumper.Maps;

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
        Console.CursorVisible = false;

        // Prototype
        Robot prototype = new Robot();

        Robot enemy1 = (Robot)prototype.Clone();
        Robot enemy2 = (Robot)prototype.Clone();
        Robot enemy3 = (Robot)prototype.Clone();

        monsters.Add((5, 10, enemy1));
        monsters.Add((10, 30, enemy2));
        monsters.Add((15, 50, enemy3));

        Stopwatch stopwatch = new Stopwatch();

        while (isRunning)
        {
            stopwatch.Restart();

            HandleInput();
            Update();
            Render();

            stopwatch.Stop();

            double sleepTime = FrameTime - stopwatch.Elapsed.TotalMilliseconds;

            if (sleepTime > 0)
                Thread.Sleep((int)sleepTime);
        }
    }

    private void HandleInput()
    {
        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);

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

            heroY += dy;
            heroX += dx;
        }
    }

    private void Update()
    {
    }

    private void Render()
    {
        Console.Clear();

        Console.SetCursorPosition(heroX, heroY);
        Console.Write("🧍");

        foreach (var monster in monsters)
        {
            Console.SetCursorPosition(monster.x, monster.y);
            Console.Write(monster.monster.GetSymbol());
        }

        Console.SetCursorPosition(0, 0);
        Console.Write($"Gold: {gold}");
    }
}