using System;
using PortalJumper.Entities;

namespace PortalJumper.Core;

public class ConsoleHUD
{
    public void Bind(Hero hero)
    {
        hero.OnHealthChanged += DisplayHealthUpdate;
    }

    private void DisplayHealthUpdate(int current, int max)
    {
        Console.SetCursorPosition(0, 22);
        string bar = new string('|', current / 10);
        string dots = new string('.', (max - current) / 10);
        Console.WriteLine($"[OBSERVER NOTIFICATION]: HP Changed to {current}! [{bar}{dots}]");
    }
}