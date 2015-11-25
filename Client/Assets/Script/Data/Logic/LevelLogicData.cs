using System.Collections.Generic;

public class LevelLogicData
{
    public int ID { get { return this.m_LevelData.ID; } }
    public string Name { get { return this.m_LevelData.Name; } }
    public string Description { get { return this.m_LevelData.Description; } }
    public string Icon { get { return this.m_LevelData.Icon; } }
    public string Map { get { return this.m_LevelData.Map; } }
    //public Dictionary<byte,ActorLogicData> BattleArray { get { return this.m_BattleArray; } }
    public Dictionary<byte,int> BattleArray { get { return this.m_LevelData.BattleArray; } }
    public WeatherType Weather { get { return this.m_LevelData.Weather; } }

    private LevelData m_LevelData;
    private Dictionary<byte, ActorLogicData> m_BattleArray;
    public LevelLogicData(LevelData levelData)
    {
        this.m_LevelData = levelData;
        /*
        this.m_BattleArray = new Dictionary<byte, ActorLogicData>();
        string actorUIDPrefix = "enemy{0}";
        string skillUIDPrefix = "enemyskill{0}";
        int actorUIDCounter = 1;
        int skillUIDCounter = 1;
        foreach (var item in levelData.BattleArray)
        {
            ActorData actorData = ConfigCtrller.Instance.Actor.GetActorDataByID(item.Value);
                List<SkillLogicDataBase> skillList = new List<SkillLogicDataBase>();
            foreach (var acotrSkillID in actorData.Skills)
            {
                ActorSkillData actorSkillData = ConfigCtrller.Instance.Skill.GetActorSkillDataByID(acotrSkillID);
                List<SkillDataBase> skillDataBaseList = new List<SkillDataBase>();
                foreach (var skillID in actorSkillData.SkillIDs)
                {
                    SkillDataBase skillDataBase = ConfigCtrller.Instance.Skill.GetSkillDataBaseByID(skillID);
                    skillDataBaseList.Add(skillDataBase);
                }
                SkillLogicDataBase skillLogicData = null;
                string skillUID = string.Format(skillUIDPrefix, skillUIDCounter);
                switch (actorSkillData.SkillType)
                {
                    case SkillType.Normal:
                        break;
                    case SkillType.First:
                    case SkillType.Active:
                        skillLogicData = new ActiveSkillLogicData(skillUID, actorSkillData, skillDataBaseList);
                        break;
                    case SkillType.Passive:
                        skillLogicData = new PassiveSkillLogicData(skillUID, actorSkillData, skillDataBaseList);
                        break;
                    case SkillType.Trigger:
                        skillLogicData = new TriggerSkillLogicData(skillUID, actorSkillData, skillDataBaseList);
                        break;
                    case SkillType.Weather:
                        skillLogicData = new WeatherSkillLogicData(skillUID, actorSkillData, skillDataBaseList);
                        break;
                    default:
                        break;
                }
                skillUIDCounter++;
                if (skillLogicData != null)
                {
                    skillList.Add(skillLogicData);
                }
            }
            ActorLogicData actorLogicData = new ActorLogicData(string.Format(actorUIDPrefix,actorUIDCounter++),actorData, skillList);
            this.m_BattleArray.Add(item.Key,actorLogicData);
        }
        */
    }
}
