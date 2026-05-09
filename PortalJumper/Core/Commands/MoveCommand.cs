namespace PortalJumper.Core.Commands;

using PortalJumper.Core.Interfaces;
using PortalJumper.Entities;

public class MoveCommand : ICommand
{
    private readonly Hero _hero;
    private readonly int _dy;
    private readonly int _dx;

    public MoveCommand(Hero hero, int dy, int dx)
    {
        _hero = hero;
        _dy = dy;
        _dx = dx;
    }

    public void Execute()
    {
        GameManager.Instance.MoveHero(_dy, _dx);
    }
}