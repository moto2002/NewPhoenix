using System.Collections.Generic;

public class LevelData
{
    public int ID;
    public string Name;
    public string Description;
    public string Icon;
    public string Map;//地图
    public Dictionary<byte, int> BattleArray;//阵容
    public WeatherType Weather;//天气，默认为晴天
    public int[] Awards;//奖励
}
