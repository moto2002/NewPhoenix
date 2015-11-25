using System.Collections.Generic;

public class SkillTriggerBuffLogicData : SkillLogicDataBase
    {
    public SkillTriggerBuffLogicData(ActorSkillData actorSkillData, List<SkillDataBase> skillList) :
       base(actorSkillData, skillList)
    {

    }
    public SkillTriggerBuffLogicData(string uid, ActorSkillData actorSkillData, List<SkillDataBase> skillList) :
        base(uid, actorSkillData, skillList)
    {

    }
}
