namespace PortalJumper.Core.Interfaces;

public interface IGameState
{
    void Update();
    void Render();
    void HandleInput();
}