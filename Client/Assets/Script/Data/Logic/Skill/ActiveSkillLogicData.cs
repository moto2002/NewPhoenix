using System.Collections.Generic;

public class ActiveSkillLogicData:SkillLogicDataBase
    {

    public ActiveSkillLogicData( ActorSkillData actorSkillData, List<SkillDataBase> skillList) :
       base( actorSkillData, skillList)
    {

    }
    public ActiveSkillLogicData(string uid, ActorSkillData actorSkillData, List<SkillDataBase> skillList) :
        base(uid, actorSkillData, skillList)
    {

    }
}
