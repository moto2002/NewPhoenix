using System.Collections.Generic;
using System.Linq;

public sealed class ActorModule
{
    /// <summary>
    /// UID,index
    /// </summary>
    private Dictionary< byte,long> m_PlayerBattleArrayDic;
    private Dictionary<long, ActorLogicData> m_ActorLogicDic;
    private Dictionary<int, ActorData> m_ActorConfigDic;

    public ActorModule()
    {
        List<ActorData> actorList = ConfigCtrller.Instance.Actor.GetActors();
        int actorUID= 1;
        int skillUID = 1;
        foreach (var actorData in actorList)
        {
            List<SkillLogicDataBase> skillLogicDataList = new List<SkillLogicDataBase>();
            foreach (var acotrSkillID in actorData.Skills)
            {
                ActorSkillData actorSkillData = ConfigCtrller.Instance.Skill.GetActorSkillDataWithID(acotrSkillID);
                List<SkillDataBase> skillDataBaseList = new List<SkillDataBase>();
                foreach (var skillID in actorSkillData.SkillIDs)
                {
                    SkillDataBase skillDataBase = ConfigCtrller.Instance.Skill.GetSkillDataBaseWithID(skillID);
                    skillDataBaseList.Add(skillDataBase);
                }
                SkillLogicDataBase skillLogicDataBase = null;
                switch (actorSkillData.SkillType)
                {
                    case SkillType.Normal:
                        break;
                    case SkillType.Active:
                        skillLogicDataBase = new actorskilllog("skill" + skillUID, actorSkillData, skillDataBaseList);
                        
                        break;
                    case SkillType.Passive:
                        break;
                    case SkillType.Trigger:
                        break;
                    case SkillType.Weather:
                        break;
                    case SkillType.First:
                        break;
                    default:
                        break;
                }
                skillUID++;

            }
            ActorLogicData actorLogicData = new ActorLogicData("actor" + actorUID++, actorData, skillLogicDataList);
        }

        this.InitBattleArray();
    }


    #region private methods

    private void InitBattleArray()
    {
        Dictionary<byte, long> battleArray = new Dictionary<byte, long>();
        battleArray.Add(1, 10000);
        battleArray.Add(2, 10001);
        battleArray.Add(3, 10002);
        battleArray.Add(6, 10003);
        battleArray.Add(7, 10004);
        battleArray.Add(8, 10005);
        this.SetBattleArray(battleArray);
    }

    #endregion

    #region public methods

    public Dictionary<byte,long> GetBattleArray()
    {
        return this.m_PlayerBattleArrayDic;
    }

    public void SetBattleArray(Dictionary< byte,long> battleArray)
    {
        this.m_PlayerBattleArrayDic = battleArray;
    }

    public ActorLogicData GetActorLogicDataByUID(long uid)
    {
        return this.m_ActorLogicDic.ContainsKey(uid) ? this.m_ActorLogicDic[uid] : null;
    }
    /*
    太尉我要向你道歉
    前天你第一次给我打电话我是看到的，我怕你是向我借钱我才故意没接的（因为上两次你打电话也是借钱，
    再加上现在比较穷，我就误解你了。），后天就出去溜达了，没带手机，你再打的几次电话，真没接到。
    由于手机没装起QQ，你发的消息也没看到，今天上班的时候才看到你说是要吃饭（我。。。。。）！白天一直在忙，
    现在才有时间组织这些话跟你道歉。你如果还在的话明天晚上请你吃饭，如果不在的话下次一定请你吃。对不起哈！
        蒋林旭 2015.11.24 01:02 
        */

    public ActorData GetActorDataByID(int id)
    {
        return this.m_ActorConfigDic.ContainsKey(id) ? this.m_ActorConfigDic[id] : null;
    }

    #endregion
}
