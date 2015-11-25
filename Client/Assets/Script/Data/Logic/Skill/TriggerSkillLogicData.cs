using System.Collections.Generic;

public class TriggerSkillLogicData : SkillLogicDataBase
    {

    public TriggerSkillLogicData(ActorSkillData actorSkillData, List<SkillDataBase> skillList) :
       base( actorSkillData, skillList)
    {

    }
    public TriggerSkillLogicData(string uid, ActorSkillData actorSkillData, List<SkillDataBase> skillList) :
        base(uid, actorSkillData, skillList)
    {

    }
}
