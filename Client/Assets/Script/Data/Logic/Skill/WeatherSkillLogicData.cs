using System.Collections.Generic;

public class WeatherSkillLogicData : SkillLogicDataBase
{
    private WeatherSkillData m_Data;
    public WeatherSkillLogicData(ActorSkillData actorSkillData, List<SkillDataBase> skillList) 
        : base(actorSkillData, skillList)
    {
        this.m_Data = (WeatherSkillData)skillList[0];
    }
    public WeatherSkillLogicData(string uid, ActorSkillData actorSkillData,List<SkillDataBase> skillList) 
        : base(uid, actorSkillData,skillList)
    {
        this.m_Data = (WeatherSkillData)skillList[0];
    }
    public WeatherType Weather { get { return this.m_Data.Weather; } }//天气类型

}
