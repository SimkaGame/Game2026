using System;
using System.Text;
using PortalJumper.Core;

class Program
{
    static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        GameManager.Instance.Run();
    }
}