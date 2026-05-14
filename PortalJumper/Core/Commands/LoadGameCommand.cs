namespace PortalJumper.Core.Commands;

using PortalJumper.Core.Interfaces;

public class LoadGameCommand : ICommand
{
    private readonly GameManager _gameManager;

    public LoadGameCommand(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Execute() => _gameManager.LoadGame();
}