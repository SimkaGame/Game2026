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
        try
        {
            int hudLine = 17; 
            
            if (hudLine < Console.BufferHeight && hudLine >= 0)
            {
                Console.SetCursorPosition(0, hudLine);
                
                int barSize = Math.Max(0, current / 10);
                int dotSize = Math.Max(0, (max - current) / 10);
                
                string bar = new string('|', barSize);
                string dots = new string('.', dotSize);
                
                Console.Write($"[OBSERVER NOTIFICATION]: HP Changed to {current}! [{bar}{dots}]".PadRight(Console.WindowWidth - 1));
            }
        }
        catch (ArgumentOutOfRangeException)
        {
        }
    }
}