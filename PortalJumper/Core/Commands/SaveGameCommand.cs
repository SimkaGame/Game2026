namespace PortalJumper.Core.Commands;

using PortalJumper.Core.Interfaces;

public class SaveGameCommand : ICommand
{
    private readonly GameManager _gameManager;

    public SaveGameCommand(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Execute() => _gameManager.SaveGame();
}