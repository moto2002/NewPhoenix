using System;
using System.Collections.Generic;
using System.Data;

public class LevelConfig
{
    private Dictionary<int, LevelData> m_LevelDic;
    public LevelConfig(DataTable stageTable,DataTable levelTable)
    {
        this.ParseStageTable(stageTable);
        this.ParseLevelTable(levelTable);
    }

    private void ParseStageTable(DataTable stageTable)
    { }

    private void ParseLevelTable(DataTable levelTable)
    {
        this.m_LevelDic = new Dictionary<int, LevelData>();
        foreach (DataRow row in levelTable.Rows)
        {
            LevelData data = new LevelData()
            {
                ID = int.Parse(row["ID"].ToString()),
                Name = row["Name"].ToString(),
                Description = row["Description"].ToString(),
                Icon = row["Icon"].ToString(),
                Map = row["Map"].ToString(),
                Weather = row.IsNull("Weather") ? WeatherType.Default : (WeatherType)Enum.Parse(typeof(WeatherType), row["Weather"].ToString()),
            };
            data.BattleArray = new Dictionary<byte, int>();
            string[] battleArrayStrArr = row["BattleArray"].ToString().Split('|');
            foreach (string str in battleArrayStrArr)
            {
                string[] strArr = str.Split(ConfigConst.SplitChar);
                data.BattleArray.Add(byte.Parse(strArr[0]), int.Parse(strArr[1]));
            }
            this.m_LevelDic.Add(data.ID, data);
        }
    }

    public LevelData GetLevelDataByID(int id)
    {
        if (this.m_LevelDic.ContainsKey(id))
        {
            return this.m_LevelDic[id];
        }
        return null;
    }
}
