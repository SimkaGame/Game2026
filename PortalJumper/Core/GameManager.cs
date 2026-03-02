using System;
using System.Threading;

namespace PortalJumper.Core;

public class GameManager
{

    private static readonly Lazy<GameManager> lazy = new(() => new GameManager());
    public static GameManager Instance => lazy.Value;

    private bool isRunning = true;

    public int MapWidth  { get; } = 50;
    public int MapHeight { get; } = 25;

    
    public void Run()
    {
        Console.WriteLine("Portal Jumper");
        Console.WriteLine($"World size: {MapWidth} × {MapHeight}");
        Console.WriteLine("Нажмите Esc для выхода\n");

        while (isRunning)
        {
            HandleInput();
            Update();
            Render();
            Thread.Sleep(16);   
        }

        Console.Clear();
        Console.WriteLine("Игра завершена.");
    }

    private void HandleInput()
    {
        if (Console.KeyAvailable)
        {
            var keyInfo = Console.ReadKey(true);  

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                isRunning = false;
            }
        }               
    }

    private void Update()
    {

    }

    private void Render()
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("Portal Jumper");
        Console.WriteLine("Esc — выход");
    }
}