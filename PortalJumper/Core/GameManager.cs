using System;

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