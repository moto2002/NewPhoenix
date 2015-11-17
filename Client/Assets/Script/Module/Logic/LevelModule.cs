using System.Collections.Generic;

public sealed class LevelModule
{
    private Dictionary<int, LevelLogicData> m_LevelDic;
    public int CurLevelID { get; private set; }

    public LevelModule()
    {
        this.InitLevel();
    }

    #region private methods

    private void InitLevel()
    {
        Dictionary<byte,int> level0BattleArray = new Dictionary<byte,int>();
        level0BattleArray.Add(0,2000);
        level0BattleArray.Add(5,2001);
        level0BattleArray.Add(6,2002);
        level0BattleArray.Add(7,2003);
        level0BattleArray.Add(8,2004);
        level0BattleArray.Add(9,2005);
        LevelData levelData0 = new LevelData()
        {
            ID = 1000,
            BattleArray = level0BattleArray,
            Weather = WeatherType.Default
        };
        LevelLogicData levelLogicData0 = new LevelLogicData(levelData0);
        this.m_LevelDic = new Dictionary<int, LevelLogicData>();
        this.m_LevelDic.Add(1000,levelLogicData0);
    }

    #endregion

    #region public methods

    public LevelLogicData GetLevelLogicDataByID(int id)
    {
        return this.m_LevelDic.ContainsKey(id) ? this.m_LevelDic[id] : null;
    }

    public LevelLogicData GetCurLevelLogicData()
    {
        if (this.CurLevelID == 0)
            return null;
        return this.GetLevelLogicDataByID(this.CurLevelID);
    }

    #endregion
}
