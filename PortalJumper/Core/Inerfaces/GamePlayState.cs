namespace PortalJumper.Core.States;

using System;
using PortalJumper.Core.Interfaces;

public class GamePlayState : IGameState
{
    private readonly GameManager _gm;
    public GamePlayState(GameManager gm) => _gm = gm;

    public void HandleInput()
    {
        var keyInfo = Console.ReadKey(true);
        var key = keyInfo.Key;

        if (key == ConsoleKey.Escape)
        {
            _gm.SetState(new PauseState(_gm));
            return;
        }

        if (key == ConsoleKey.NoName || key == 0)
        {
            char ch = char.ToLower(keyInfo.KeyChar);
            key = ch switch
            {
                'w' or 'ц' => ConsoleKey.W,
                's' or 'ы' => ConsoleKey.S,
                'a' or 'ф' => ConsoleKey.A,
                'd' or 'в' => ConsoleKey.D,
                'k' or 'л' => ConsoleKey.K,
                'l' or 'д' => ConsoleKey.L,
                _ => key
            };
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
        var keyInfo = Console.ReadKey(true);
        if (keyInfo.Key == ConsoleKey.Escape || keyInfo.KeyChar == '')
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
        while (Console.KeyAvailable) 
        {
            Console.ReadKey(true);
        }
    }

    public void Update() { }

    public void Render() => _gm.ShowGameOverScreen();
}