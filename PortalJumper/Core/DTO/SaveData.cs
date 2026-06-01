using System.Collections.Generic;

namespace PortalJumper.Core.DTO;

public class SaveData
{
    public int HeroHp { get; set; }
    public int HeroMaxHp { get; set; }
    public int HeroGold { get; set; }
    public int HeroX { get; set; }
    public int HeroY { get; set; }
    public int CurrentLevel { get; set; }
    public PositionDto? PortalPos { get; set; }
    
    public List<MonsterSaveDto> Monsters { get; set; } = new();
    public List<PositionDto> Coins { get; set; } = new();
    public PositionDto? ShieldPos { get; set; }
    public PositionDto? WindPos { get; set; }
}

public class MonsterSaveDto
{
    public string Type { get; set; } = null!;
    public int X { get; set; }
    public int Y { get; set; }
}

public class PositionDto
{
    public int X { get; set; }
    public int Y { get; set; }
}