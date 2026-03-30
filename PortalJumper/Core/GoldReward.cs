namespace PortalJumper.Core;
using PortalJumper.Entities;

public class GoldReward : IReward, ICloneable
{
    public int Amount { get; set; }
    public Position Position { get; set; }

    public GoldReward(int amount = 10)
    {
        Amount = amount > 0 ? amount : 10;
        Position = new Position(0, 0);
    }

    public string GetSymbol() => "💰";

    public void Collect(Hero hero)
    {
        hero.Gold += Amount;
    }

    public object Clone()
    {
        return new GoldReward(Amount)
        {
            Position = new Position(Position.Y, Position.X)
        };
    }
}