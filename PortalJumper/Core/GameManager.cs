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


        while (isRunning)
        {
            
        }
    }

    private void HandleInput()
    {
        
    }

    private void Update()
    {
        
    }

    private void Render()
    {


    }
}