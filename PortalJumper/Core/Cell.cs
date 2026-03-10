namespace PortalJumper.Core;
using PortalJumper.Entities;

public class Cell
{
    public Position Position { get; }
    public bool IsPassable { get; }
    public List<IReward> Rewards { get; } = new();

    public Cell(Position position, bool isPassable)
    {
        Position = position;
        IsPassable = isPassable;
    }

    public void AddReward(IReward reward) => Rewards.Add(reward);

    public void CollectAllRewards(Hero hero)
    {
        foreach (var reward in Rewards)
            reward.Collect(hero);

        Rewards.Clear();
    }
}