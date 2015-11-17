public class WeatherSkillLogicData : SkillLogicDataBase
{
    private WeatherSkillData m_Data;
    public WeatherSkillLogicData(string uid, WeatherSkillData data) : base(uid, data)
    {
        this.m_Data = data;
    }
    public WeatherType Weather { get { return this.m_Data.Weather; } }//天气类型

}
