using System.Collections.Generic;
using System.Linq;

public sealed class ActorModule
{
    /// <summary>
    /// UID,index
    /// </summary>
    private Dictionary< byte,long> m_PlayerBattleArrayDic;
    private Dictionary<long, ActorLogicData> m_ActorLogicDic;
    public Dictionary<int, ActorData> m_ActorConfigDic;

    public ActorModule()
    {
        //Dictionary<byte,long> battleArray = new Dictionary< byte,long>();
        //battleArray.Add(1,10000);
        //battleArray.Add(2,10001);
        //battleArray.Add(3,10002);
        //battleArray.Add(6,10003);
        //battleArray.Add(7,10004);
        //battleArray.Add(8,10005);
        //this.SetBattleArray(battleArray);
    }

    #region private methods

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

    public ActorData GetActorDataByID(int id)
    {
        return this.m_ActorConfigDic.ContainsKey(id) ? this.m_ActorConfigDic[id] : null;
    }

    #endregion
}
