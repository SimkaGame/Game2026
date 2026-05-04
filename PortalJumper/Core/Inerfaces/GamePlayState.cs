namespace PortalJumper.Core.States;

using System;
using PortalJumper.Core.Interfaces;

public class GamePlayState : IGameState
{
    private readonly GameManager _gm;
    public GamePlayState(GameManager gm) => _gm = gm;

    public void HandleInput()
    {
        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.Escape)
        {
            _gm.SetState(new PauseState(_gm));
            return;
        }
        _gm.ProcessHeroInput(key);
    }

    public void Update()
    {
        _gm.UpdateGameLogic();
        if (_gm.GetHeroHp() <= 0) _gm.SetState(new GameOverState(_gm));
    }

    public void Render() => _gm.RenderGame();
}

public class PauseState : IGameState
{
    private readonly GameManager _gm;
    public PauseState(GameManager gm) => _gm = gm;

    public void HandleInput()
    {
        if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            _gm.SetState(new GamePlayState(_gm));
    }

    public void Update() { }

    public void Render()
    {
        _gm.RenderGame();
        Console.SetCursorPosition(10, 7);
        Console.WriteLine("=== ПАУЗА (ESC - ПРОДОЛЖИТЬ) ===");
    }
}

public class GameOverState : IGameState
{
    private readonly GameManager _gm;
    public GameOverState(GameManager gm) => _gm = gm;

    public void HandleInput()
    {
        if (Console.KeyAvailable) Console.ReadKey(true);
    }

    public void Update() { }

    public void Render() => _gm.ShowGameOverScreen();
}