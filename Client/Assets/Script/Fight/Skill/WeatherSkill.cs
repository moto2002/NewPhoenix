using System;
using System.Collections.Generic;

public class WeatherSkill : SkillBase
{
    #region override methods
    private WeatherSkillLogicData m_WeatherSkillLogicData;
    public override void Init(ActorBevBase actor, SkillLogicDataBase data)
    {
        base.Init(actor, data);
        this.m_WeatherSkillLogicData = (WeatherSkillLogicData)data;
    }

    public override void Moves()
    {
        this.Actor.ShowWeather(this.m_WeatherSkillLogicData.Weather);
    }

    protected override List<ActorBevBase> FindTarget()
    {
        throw new NotImplementedException();
    }

    #endregion


}
