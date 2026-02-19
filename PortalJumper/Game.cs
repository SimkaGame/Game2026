using System;

namespace PortalJumper
{
    public class Game
    {
        private bool isRunning = true;

        public void Run()
        {
            while (isRunning)
            {
                HandleInput();
                Update();
                Render();
                Thread.Sleep(40);
            }
        }

        private void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
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
            Console.Clear();
            Console.WriteLine("Portal Jumper");
            Console.WriteLine("Esc - Выход");
        }
    }
}