using System.Collections.Generic;

public class PassiveSkillLogicData : SkillLogicDataBase
    {
    public PassiveSkillLogicData( ActorSkillData actorSkillData, List<SkillDataBase> skillList) :
       base( actorSkillData, skillList)
    {

    }
    public PassiveSkillLogicData(string uid, ActorSkillData actorSkillData, List<SkillDataBase> skillList) :
        base(uid, actorSkillData, skillList)
    {

    }
}
