using System.Collections.Generic;
using System.Linq;

public sealed class ActorModule
{
    /// <summary>
    /// UID,index
    /// </summary>
    private Dictionary< byte,string> m_PlayerBattleArrayDic;
    private Dictionary<string , ActorLogicData> m_ActorLogicDataDic;

    public ActorModule()
    {
        this.m_ActorLogicDataDic = new Dictionary<string, ActorLogicData>();
        List<ActorData> actorList = ConfigCtrller.Instance.Actor.GetActors();
        string actorUIDPrefix= "actor{0}";
        string skillUIDPrefix= "skill{0}";
        int actorUIDCounter= 1;
        int skillUIDCounter = 1;
        foreach (var actorData in actorList)
        {
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
            string actorUID = string.Format(actorUIDPrefix,actorUIDCounter++);
            ActorLogicData actorLogicData = new ActorLogicData(actorUID, actorData, skillList);
            this.m_ActorLogicDataDic.Add(actorUID,actorLogicData);
        }
        this.InitBattleArray();
    }

    #region private methods

    private void InitBattleArray()
    {
        Dictionary<byte, string > battleArray = new Dictionary<byte, string>();
        battleArray.Add(1, "actor1");
        battleArray.Add(2, "actor2");
        battleArray.Add(3, "actor3");
        battleArray.Add(6, "actor4");
        battleArray.Add(7, "actor5");
        battleArray.Add(8, "actor6");
        this.SetBattleArray(battleArray);
    }

    #endregion

    #region public methods

    public Dictionary<byte,string> GetBattleArray()
    {
        return this.m_PlayerBattleArrayDic;
    }

    public void SetBattleArray(Dictionary< byte,string> battleArray)
    {
        this.m_PlayerBattleArrayDic = battleArray;
    }

    public ActorLogicData GetActorLogicDataByUID(string uid)
    {
        return this.m_ActorLogicDataDic.ContainsKey(uid) ? this.m_ActorLogicDataDic[uid] : null;
    }
    /*
    太尉我要向你道歉
    前天你第一次给我打电话我是看到的，我怕你是向我借钱我才故意没接的（因为上两次你打电话也是借钱，
    再加上现在比较穷，我就误解你了。），后天就出去溜达了，没带手机，你再打的几次电话，真没接到。
    由于手机没装起QQ，你发的消息也没看到，今天上班的时候才看到你说是要吃饭（我。。。。。）！白天一直在忙，
    现在才有时间组织这些话跟你道歉。你如果还在的话明天晚上请你吃饭，如果不在的话下次一定请你吃。对不起哈！
        蒋林旭 2015.11.24 01:02 
        */

    #endregion
}
