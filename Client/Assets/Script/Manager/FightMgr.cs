using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


/*
2015.10.28-log:
声明：
    1.Round 游戏回合
    2.Actor 角色
    3.ActorMoves 角色出招，即角色的“回合”
    4.ActorAttack   角色攻击，即普通攻击或者技能攻击
*/

public class FightMgr : MonoBehaviour
{
    public static FightMgr Instance { get; private set; }
    public Transform EnemyParent;
    public Transform PlayerParent;
    public GridData[,] EnemyGrids { get { return this.m_GridComp.EnemyGrids; } }
    public GridData[,] PlayerGrids { get { return this.m_GridComp.PlayerGrids; } }
    public List<ActorBevBase> EnemyList { get; private set; }//敌人列表
    public List<ActorBevBase> PlayerList { get; private set; }//玩家列表
    public List<ActorBevBase> AllActorList { get; private set; }//场上所有角色列表
    public WeatherType CurWeather { get; private set; }

    private FightGridComponent m_GridComp;//网格
    private byte m_RandCount;//回合数
    private bool m_GameOver;//是否游戏结束
    private ActorBevBase m_CurMovesActor;//当前出招的角色
    private ActorBevBase m_WeatherCaster;//施放天气的角色
    private bool m_PlayerInitComplete;//玩家初始化完成
    private bool m_EnemyInitComplete;//敌人初始化完成
    private LevelLogicData m_CurLevelLogicData;//当前关卡数据
    private List<ActorBevBase> m_FirstSkillActorList;//先机技能玩家列表


    #region MonoBehaviour methods

    void Awake()
    {
        Instance = this;
    }
    IEnumerator Start()
    {
        this.m_CurLevelLogicData = LogicCtrller.Instance.Level.GetCurLevelLogicData();
        yield return StartCoroutine(this.Init());
        this.CreateWeather();
        yield return new WaitForSeconds(1.0f);
        this.FirstSkill();
    }

    #endregion

    #region private methods
    /// <summary>
    /// 复制预物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    private T InstantiateActor<T>(byte index, int id) where T : ActorBevBase
    {
        ActorData actorData = ConfigCtrller.Instance.Actor.GetActorDataByID(id);
        GameObject actor = PoolMgr.Instance.GetModel(AssetPathConst.Actor + actorData.Model);
        TransUtils.ChangeLayer(actor.transform, LayerConst.Actor);
        T bev = actor.AddComponent<T>();
        bev.Index = index;
        return bev;
    }

    private bool EnableMoves(ActorBevBase actor)
    {
        return !(actor.IsDead || actor.IsMoves());
    }

    /// <summary>
    /// 计算玩家或地方得到最符合条件的下一个出招的角色
    /// </summary>
    /// <returns></returns>
    private ActorBevBase GetMovesActor(List<ActorBevBase> actorList)
    {
        List<ActorBevBase> enableMovesActors = actorList.Where(a => this.EnableMoves(a)).ToList();
        if (enableMovesActors.Count == 0)
            return null;
        if (enableMovesActors.Count == 1)
            return enableMovesActors[0];

        float maxAttackSpeed = enableMovesActors.Max(a => a.ActorLogicData.GetAttackSpeed());
        List<ActorBevBase> maxSpeedActors = enableMovesActors.Where(a => a.ActorLogicData.GetAttackSpeed() == maxAttackSpeed).ToList();
        if (maxSpeedActors.Count == 1)
            return maxSpeedActors[0];

        int fightRandomValueForMovesActor_Index = Random.Range(0, maxSpeedActors.Count);
        return maxSpeedActors[fightRandomValueForMovesActor_Index];
    }



    /// <summary>
    /// 重置敌人和玩家
    /// </summary>
    private void ResetActorMovesState()
    {
    }

    #endregion

    #region public methods
    #endregion

    #region 游戏流程

    #region 0:Init

    /// <summary>
    /// 初始化
    /// </summary>
    private IEnumerator Init()
    {
        this.InitScene();
        yield return null;
        StartCoroutine(this.InitPlayer());
        StartCoroutine(this.InitEnemy());
        while (!(this.m_PlayerInitComplete && this.m_EnemyInitComplete))
        {
            yield return 2;
        }
        this.AllActorList = new List<ActorBevBase>();
        this.AllActorList.AddRange(this.PlayerList);
        this.AllActorList.AddRange(this.EnemyList);
        this.InitUI();
    }
    /// <summary>
    /// 初始化玩家
    /// </summary>
    private IEnumerator InitPlayer()
    {
        this.PlayerList = new List<ActorBevBase>();
        foreach (KeyValuePair<byte, string> kv in LogicCtrller.Instance.Actor.GetBattleArray())
        {
            byte index = kv.Key;
            string uid = kv.Value;
            GridData gridData = this.m_GridComp.ConvertIndexToGridData(index);
            PlayerBev playerBev = this.InstantiateActor<PlayerBev>(index, LogicCtrller.Instance.Actor.GetActorLogicDataByUID(uid).ID);
            playerBev.transform.SetParent(this.PlayerParent);
            playerBev.transform.position = this.m_GridComp.ConvertGridToPosition(gridData, ActorType.Player);
            playerBev.transform.localRotation = Quaternion.identity;
            playerBev.transform.localScale = Vector3.one;
            playerBev.InitPlayer(uid, gridData);
            this.PlayerList.Add(playerBev);
            this.AllActorList.Add(playerBev);
            yield return null;
        }
        this.m_PlayerInitComplete = true;
    }

    /// <summary>
    /// 初始化敌人
    /// </summary>
    private IEnumerator InitEnemy()
    {
        this.EnemyList = new List<ActorBevBase>();
        LevelLogicData levelLogicData = LogicCtrller.Instance.Level.GetCurLevelLogicData();
        foreach (KeyValuePair<byte, int> kv in levelLogicData.BattleArray)
        {
            byte index = kv.Key;
            int id = kv.Value;
            GridData gridData = this.m_GridComp.ConvertIndexToGridData(index);
            EnemyBev enemyBev = this.InstantiateActor<EnemyBev>(index, id);
            enemyBev.transform.SetParent(this.EnemyParent);
            enemyBev.transform.position = this.m_GridComp.ConvertGridToPosition(gridData, ActorType.Enemy);
            enemyBev.transform.localRotation = Quaternion.identity;
            enemyBev.transform.localScale = Vector3.one;
            enemyBev.InitEnemy(id, gridData);
            this.EnemyList.Add(enemyBev);
            this.AllActorList.Add(enemyBev);
            yield return null;
        }
        this.m_PlayerInitComplete = true;
    }

    /// <summary>
    /// 初始化场景
    /// </summary>
    private void InitScene()
    {
        this.m_GridComp = this.GetComponent<FightGridComponent>();
    }
    /// <summary>
    /// 初始化UI
    /// </summary>
    private void InitUI()
    {
        UICtrller.Instance.Open(PanelType.Fight);
    }

    #endregion

    #region 1:Weather

    /// <summary>
    /// 创建天气
    /// </summary>
    private void CreateWeather()
    {
        List<ActorBevBase> weatherCasters = this.AllActorList.Where(a => a.HasWeatherSkill).ToList();
        if (weatherCasters == null || weatherCasters.Count == 0)
        {
            //并没有天气技能
            this.SetLevelWeather();
        }
        else
        {
            foreach (var item in weatherCasters)
            {
                item.CastWeatherSkill();
                item.RemoveWeatherSkill();
            }
            ActorBevBase caster = null;
            if (weatherCasters.Count == 1)
            {
                caster = weatherCasters[0];

            }
            else
            {
                float maxAttackSpeed = weatherCasters.Max(a => a.ActorLogicData.GetAttackSpeed());
                List<ActorBevBase> maxSpeedActors = weatherCasters.Where(a => a.ActorLogicData.GetAttackSpeed() == maxAttackSpeed).ToList();
                if (maxSpeedActors.Count == 1)
                {
                    caster = maxSpeedActors[0];
                }
                else
                {
                    int fightRandomValueForWeatherActor_Index = Random.Range(0, maxSpeedActors.Count());
                    caster = maxSpeedActors[fightRandomValueForWeatherActor_Index];
                }
            }
            this.CastWeatherSkill(caster);
        }
    }

    /// <summary>
    /// 施放天气 
    /// </summary>
    private void CastWeatherSkill(ActorBevBase caster)
    {
        WeatherType? weather = caster.GetWeatherFromWeatherSkill;
        if (weather.HasValue)
        {
            Debug.Log("天气胜利者：" + caster.name);
            this.m_WeatherCaster = caster;
            this.SetWeather(weather.Value);
        }
        else
        {
            this.SetLevelWeather();
        }
    }

    private void SetLevelWeather()
    {
        this.SetWeather(LogicCtrller.Instance.Level.GetCurLevelLogicData().Weather);
    }

    private void SetWeather(WeatherType weather)
    {
        this.CurWeather = weather;
        switch (weather)
        {
            case WeatherType.Default:
                Debug.Log("默认天气");
                break;
            case WeatherType.Wind:
                Debug.Log("天气：风");
                break;
            case WeatherType.Lightning:
                Debug.Log("天气：雷电");
                break;
            case WeatherType.Sandstorm:
                Debug.Log("天气：沙暴");
                break;
            case WeatherType.Haze:
                Debug.Log("天气：雾霾");
                break;
            default:
                Debug.Log("未知天气");
                break;
        }
    }

    #endregion

    #region 2:FirstSkill

    /// <summary>
    /// 先机技能
    /// </summary>
    private void FirstSkill()
    {
        this.FirstSkillStart();
        this.FirstSkillRounding();
    }

    private void FirstSkillStart()
    {
        this.m_FirstSkillActorList = this.AllActorList.Where(a => a.HasFirstSkill).ToList();
        if (this.m_FirstSkillActorList == null || this.m_FirstSkillActorList.Count == 0)
        {
            //并没有先机技能
            this.RoundStart();
        }
        else
        {
            this.FirstSkillRounding();
        }
    }

    private void FirstSkillRounding()
    {
        if (this.SearchMovesActor(this.m_FirstSkillActorList))
        {
            this.FirstSkillMoves();
        }
        else
        {
            this.FirstSkillEnd();
        }
    }

    private void FirstSkillMoves()
    {
        this.m_CurMovesActor.CastFirstSkill();
    }

    private void FirstSkillMvoesEnd()
    {
        this.FirstSkillRounding();
    }

    /// <summary>
    /// 先机技能施放完成 开始回合
    /// </summary>
    private void FirstSkillEnd()
    {
        this.RoundStart();
    }

    #endregion

    #region 4:Round

    /// <summary>
    /// 回合开始
    /// </summary>
    private void RoundStart()
    {
        //回合数加一 
        this.m_RandCount++;

        this.Rounding();
    }
    /// <summary>
    /// 回合中
    /// </summary>
    private void Rounding()
    {
        if (this.SearchMovesActor(this.AllActorList))
        {
            this.RoundEnd();
        }
        else
        {
            this.ActorMoves();
        }
    }

    /// <summary>
    /// 回合结束
    /// </summary>
    private void RoundEnd()
    {
        if (this.CheckIsGameOver())
        {
            this.GameOver();
        }
        else
        {

            //重置参数
            this.ResetActorMovesState();
            //进入下一回合
            this.RoundStart();
        }
    }

    /// <summary>
    /// 搜索出手角色
    /// </summary>
    /// <returns></returns>
    private bool SearchMovesActor(List<ActorBevBase> actorList)
    {
        this.m_CurMovesActor = this.GetMovesActor(actorList);
        return this.m_CurMovesActor != null;
    }

    private void ActorMoves()
    {
        this.m_CurMovesActor.Moves();
    }

    private void ActorMovesEnd()
    {
        this.m_CurMovesActor = null;
        this.Rounding();
    }

    #endregion

    #region 5:Settlement

    /// <summary>
    /// 检测游戏是否结束
    /// </summary>
    /// <returns></returns>
    private bool CheckIsGameOver()
    {
        return this.m_RandCount >= FightConst.MaxRoundCount;
    }
    /// <summary>
    /// 游戏结束
    /// </summary>
    private void GameOver()
    {
        this.m_GameOver = true;
        if (this.CheckResult())
        {
            this.Victory();
        }
        else
        {
            this.Fail();
        }
    }
    /// <summary>
    /// 检测输赢
    /// </summary>
    /// <returns></returns>
    private bool CheckResult()
    {
        return false;
    }
    /// <summary>
    /// 胜利
    /// </summary>
    private void Victory()
    {
        UICtrller.Instance.Open(PanelType.Victory);
    }
    /// <summary>
    /// 失败
    /// </summary>
    private void Fail()
    {
        UICtrller.Instance.Open(PanelType.Fail);
    }


    #endregion
    #endregion 游戏流程

    #region Grid

    public byte GetMaxMagnitudeDistance()
    {
        return this.m_GridComp.GetMaxMagnitudeDistance();
    }

    public byte GetMaxIndex()
    {
        return this.m_GridComp.GetMaxIndex();
    }

    public byte CalculateMagnitudeDistance(GridData data1, GridData data2, bool isTwoCamp)
    {
        return this.m_GridComp.CalculateMagnitudeDistance(data1, data2, isTwoCamp);
    }

    #endregion

    public List<ActorBevBase> GetOtherSideActor(ActorType actorType)
    {
        switch (actorType)
        {
            case ActorType.Enemy: return this.PlayerList;
            case ActorType.Player: return this.EnemyList;
        }
        return null;
    }

    public List<ActorBevBase> GetOwnSideActor(ActorType actorType)
    {
        switch (actorType)
        {
            case ActorType.Enemy: return this.EnemyList;
            case ActorType.Player: return this.PlayerList;
        }
        return null;

    }

    #region Find 先写暂时的，以后还需要根据情况改 （可以改成工厂+策略）

    //2015.12.03-log:需要弄清楚是否需要筛选掉已经死亡的
    //2015.12.14-log:筛选掉已经死亡的

    /// <summary>
    /// 获取查找列表
    /// </summary>
    /// <param name="type"></param>
    /// <returns>查找列表</returns>
    private List<ActorBevBase> GetFindList(ActorType type)
    {
        return this.GetFindList(type == ActorType.Player);
    }

    /// <summary>
    /// 获取查找列表
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns>查找列表</returns>
    private List<ActorBevBase> GetFindList(bool isPlayer)
    {
        List<ActorBevBase> findList = null;
        if (isPlayer)
        {
            findList = this.PlayerList;
        }
        else
        {
            findList = this.EnemyList;
        }
        //筛选掉已经死亡的
        return findList.Where(a => !a.IsDead).ToList();
    }

    /// <summary>
    /// 获取查找列表
    /// </summary>
    /// <param name="sourceType">源的类型</param>
    /// <param name="effectTarget">作用阵营</param>
    /// <returns>查找列表</returns>
    private List<ActorBevBase> GetFindList(ActorType sourceType, EffectTargetType effectTarget)
    {
        bool isPlayer = (sourceType == ActorType.Enemy && effectTarget == EffectTargetType.Rival)
            || (sourceType == ActorType.Player && effectTarget == EffectTargetType.Friend);
        return this.GetFindList(isPlayer);
    }

    /// <summary>
    /// 获取所有目标列表
    /// </summary>
    /// <param name="sourceType">源的类型</param>
    /// <param name="effectTarget">作用阵营</param>
    /// <returns>所有目标列表</returns>
    public List<ActorBevBase> FindAll(ActorType sourceType, EffectTargetType effectTarget)
    {
        return this.GetFindList(sourceType, effectTarget);
    }

    /// <summary>
    /// 查找最近的目标列表
    /// </summary>
    /// <param name="sourceType">源的类型</param>
    /// <param name="sourceGrid">源的格子数据</param>
    /// <param name="count">查找数量</param>
    /// <param name="effectTarget">查找阵营</param>
    /// <returns>找到的目标列表</returns>
    public List<ActorBevBase> FindNear(ActorType sourceType, GridData sourceGrid, byte count, EffectTargetType effectTarget)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceType, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (findList.Count <= count)
        {
            return findList;
        }
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        if (effectTarget == EffectTargetType.Rival)
        {
            for (byte i = 0; i < count; i++)
            {
                ActorBevBase target = null;
                foreach (var tmpTarget in findList)
                {
                    if (target == null)
                    {
                        target = tmpTarget;
                    }
                    else
                    {
                        if (tmpTarget.GridData.ZGrid < target.GridData.ZGrid)
                        {
                            target = tmpTarget;
                        }
                        else if (tmpTarget.GridData.ZGrid == target.GridData.ZGrid
                            && Mathf.Abs(tmpTarget.GridData.XGrid - sourceGrid.XGrid) <= Mathf.Abs(tmpTarget.GridData.XGrid - sourceGrid.XGrid)
                            && tmpTarget.GridData.XGrid < target.GridData.XGrid)
                        {
                            target = tmpTarget;
                        }
                    }
                }
                findList.Remove(target);
                targetList.Add(target);
            }
        }
        else if (effectTarget == EffectTargetType.Friend)
        {
            for (byte i = 0; i < count; i++)
            {
                ActorBevBase target = null;
                byte minDistance = byte.MaxValue;
                foreach (var tmpTarget in findList)
                {
                    byte distance = (byte)(sourceGrid.CalculateDistanceMagnitude(tmpTarget.GridData));
                    if (target == null)
                    {
                        target = tmpTarget;
                        minDistance = distance;
                    }
                    else
                    {
                        if (distance < minDistance)
                        {
                            target = tmpTarget;
                            minDistance = distance;
                        }
                        else if (distance == minDistance && tmpTarget.Index < target.Index)
                        {
                            target = tmpTarget;
                            minDistance = distance;
                        }
                    }
                }
                findList.Remove(target);
                targetList.Add(target);
            }
        }
        return targetList;
    }

    /// <summary>
    /// 查找最远的目标列表
    /// </summary>
    /// <param name="sourceType">源的类型</param>
    /// <param name="sourceGrid">源的格子数据</param>
    /// <param name="count">查找数量</param>
    /// <param name="effectTarget">查找阵营</param>
    /// <returns>找到的目标列表</returns>
    public List<ActorBevBase> FindFar(ActorType sourceType, GridData sourceGrid, byte count, EffectTargetType effectTarget)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceType, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (findList.Count <= count)
        {
            return findList;
        }
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        if (effectTarget == EffectTargetType.Rival)
        {
            for (byte i = 0; i < count; i++)
            {
                ActorBevBase target = null;
                foreach (var tmpTarget in findList)
                {
                    if (target == null)
                    {
                        target = tmpTarget;
                    }
                    else
                    {
                        if (tmpTarget.GridData.ZGrid > target.GridData.ZGrid)
                        {
                            target = tmpTarget;
                        }
                        else if (tmpTarget.GridData.ZGrid == target.GridData.ZGrid
                            && Mathf.Abs(tmpTarget.GridData.XGrid - sourceGrid.XGrid) <= Mathf.Abs(tmpTarget.GridData.XGrid - sourceGrid.XGrid)
                            && tmpTarget.GridData.XGrid < target.GridData.XGrid)
                        {
                            target = tmpTarget;
                        }
                    }
                }
                findList.Remove(target);
                targetList.Add(target);
            }
        }
        else if (effectTarget == EffectTargetType.Friend)
        {
            for (byte i = 0; i < count; i++)
            {
                ActorBevBase target = null;
                byte maxDistance = byte.MaxValue;
                for (byte j = 0; j < findList.Count; j++)
                {
                    ActorBevBase tmpTarget = findList[j];
                    byte distance = sourceGrid.CalculateDistanceMagnitude(tmpTarget.GridData);
                    if (target == null)
                    {
                        target = tmpTarget;
                        maxDistance = distance;
                    }
                    else
                    {
                        if (distance > maxDistance)
                        {
                            target = tmpTarget;
                            maxDistance = distance;
                        }
                        else if (distance == maxDistance && tmpTarget.Index > target.Index)
                        {
                            target = tmpTarget;
                            maxDistance = distance;
                        }
                    }
                }
                findList.Remove(target);
                targetList.Add(target);
            }
        }
        return targetList;
    }

    /// <summary>
    /// 查找属性最小的目标列表
    /// </summary>
    /// <param name="sourceType">源的类型</param>
    /// <param name="count">查找数量</param>
    /// <param name="effectTarget">查找阵营</param>
    /// <param name="attribute">参照属性</param>
    /// <returns>找到的目标列表</returns>
    public List<ActorBevBase> FindMinAttribute(ActorType sourceType, byte count, EffectTargetType effectTarget, AttributeType attribute)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceType, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (findList.Count <= count)
        {
            return findList;
        }
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        for (byte i = 0; i < count; i++)
        {
            ActorBevBase target = null;
            foreach (var tmpTarget in findList)
            {
                if (target == null)
                {
                    target = tmpTarget;
                }
                else
                {
                    if (tmpTarget.ActorLogicData.GetAttrbuteValueByType(attribute) < target.ActorLogicData.GetAttrbuteValueByType(attribute))
                    {
                        target = tmpTarget;
                    }
                    else if (tmpTarget.ActorLogicData.GetAttrbuteValueByType(attribute) == target.ActorLogicData.GetAttrbuteValueByType(attribute)
                        && tmpTarget.Index < target.Index)
                    {
                        target = tmpTarget;
                    }
                }
            }
            findList.Remove(target);
            targetList.Add(target);
        }
        return targetList;
    }

    /// <summary>
    /// 查找属性最大的目标列表
    /// </summary>
    /// <param name="sourceType">源的类型</param>
    /// <param name="count">查找数量</param>
    /// <param name="effectTarget">查找阵营</param>
    /// <param name="attribute">参照属性</param>
    /// <returns>找到的目标列表</returns>
    public List<ActorBevBase> FindMaxAttribute(ActorType sourceType, byte count, EffectTargetType effectTarget, AttributeType attribute)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceType, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (findList.Count <= count)
        {
            return findList;
        }
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        for (byte i = 0; i < count; i++)
        {
            ActorBevBase target = null;
            foreach (var tmpTarget in findList)
            {
                if (target == null)
                {
                    target = tmpTarget;
                }
                else
                {
                    if (tmpTarget.ActorLogicData.GetAttrbuteValueByType(attribute) > target.ActorLogicData.GetAttrbuteValueByType(attribute))
                    {
                        target = tmpTarget;
                    }
                    else if (tmpTarget.ActorLogicData.GetAttrbuteValueByType(attribute) == target.ActorLogicData.GetAttrbuteValueByType(attribute)
                        && tmpTarget.Index < target.Index)
                    {
                        target = tmpTarget;
                    }
                }
            }
            findList.Remove(target);
            targetList.Add(target);
        }
        return targetList;
    }

    /// <summary>
    /// 查找对面目标列表
    /// 对面打完，打旁边列，先左后右
    /// <param name="sourceType">源的类型</param>
    /// <param name="sourceGrid">源的格子数据</param>
    /// <returns>找到的目标</returns>
    public ActorBevBase FindOpposite(ActorType sourceType, GridData sourceGrid)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceType, EffectTargetType.Rival);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        ActorBevBase target = null;
        foreach (var tmpTarget in findList)
        {
            if (target == null)
            {
                target = tmpTarget;
            }
            else
            {
                // XGrid : 0 1 2 3 4
                if (Mathf.Abs(tmpTarget.GridData.XGrid - sourceGrid.XGrid) <= Mathf.Abs(target.GridData.XGrid - sourceGrid.XGrid)
                    && tmpTarget.GridData.XGrid <= target.GridData.XGrid && tmpTarget.GridData.ZGrid < target.GridData.ZGrid)
                {
                    target = tmpTarget;
                }
            }
        }
        return target;
    }

    /// <summary>
    /// 查找随机目标列表
    /// </summary>
    /// <param name="sourceType">源的类型</param>
    /// <param name="count">查找数量</param>
    /// <param name="effectTarget">查找阵营</param>
    /// <returns>找到的目标列表</returns>
    public List<ActorBevBase> FindRandom(ActorType sourceType, byte count, EffectTargetType effectTarget)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceType, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (findList.Count <= count)
        {
            return findList;
        }
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        for (byte i = 0; i < count; i++)
        {
            ActorBevBase target = findList[Random.Range(0, findList.Count)];
            findList.Remove(target);
            targetList.Add(target);
        }
        return targetList;
    }

    /// <summary>
    /// 查找指定字段目标列表
    /// </summary>
    /// <param name="sourceType">源的类型</param>
    /// <param name="count">查找数量</param>
    /// <param name="effectTarget">查找阵营</param>
    /// <param name="field">参照字段</param>
    /// <param name="fieldValue">参照字段的值</param>
    /// <returns>找到的目标列表</returns>
    public List<ActorBevBase> FindField(ActorType sourceType, byte count, EffectTargetType effectTarget, AttributeType field, string fieldValue)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceType, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (findList.Count <= count)
        {
            return findList;
        }
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        for (byte i = 0; i < count; i++)
        {
            ActorBevBase target = null;
            foreach (var tmpTarget in findList)
            {
                if (tmpTarget.ActorLogicData.GetFieldValueByType(field).Equals(fieldValue))
                {
                    if (target == null)
                    {
                        target = tmpTarget;
                    }
                    else if (tmpTarget.Index < target.Index)
                    {
                        target = tmpTarget;
                    }
                }
            }
            findList.Remove(target);
            targetList.Add(target);
        }
        return targetList;
    }

    /// <summary>
    /// 查找最大矩形目标列表
    /// </summary>
    /// <param name="sourceType">源的类型</param>
    /// <param name="effectTarget">查找阵营</param>
    /// <param name="width">上（X）</param>
    /// <param name="height">宽(Z)</param>
    /// <returns>找到的目标列表</returns>
    public List<ActorBevBase> FindMaxRect(ActorType sourceType, EffectTargetType effectTarget, byte width, byte height)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceType, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (width >= this.m_GridComp.XGridCount && height >= this.m_GridComp.ZGridCount)
        {
            return findList;
        }

        //List<ActorBevBase> targetList = null;
        //for (byte x = 0; x <= this.m_GridComp.XGridCount - width; x++)
        //{
        //    for (byte z = 0; z <= this.m_GridComp.ZGridCount - height; z++)
        //    {
        //        List<ActorBevBase> tmpTargetList = new List<ActorBevBase>();
        //        foreach (var tmpTarget in findList)
        //        {
        //            if (x <= tmpTarget.GridData.XGrid && tmpTarget.GridData.XGrid < x + width
        //                && z <= tmpTarget.GridData.ZGrid && tmpTarget.GridData.ZGrid < z + height)
        //            {
        //                tmpTargetList.Add(tmpTarget);
        //            }
        //        }
        //        if (targetList == null || tmpTargetList.Count > targetList.Count)
        //        {
        //            targetList = tmpTargetList;
        //        }
        //    }
        //}
        //return targetList;

        return this.GetTargetListWithRect(findList, 0, (byte)(this.m_GridComp.XGridCount - width), 0, (byte)(this.m_GridComp.ZGridCount - height), width, height);
    }

    /// <summary>
    /// 查找最大十字形目标列表
    /// </summary>
    /// <param name="sourceType">源的类型</param>
    /// <param name="effectTarget">查找阵营</param>
    /// <param name="top">上</param>
    /// <param name="bottom">下</param>
    /// <param name="left">左</param>
    /// <param name="right">右</param>
    /// <returns>找到的目标列表</returns>
    public List<ActorBevBase> FindMaxCross(ActorType sourceType, EffectTargetType effectTarget, byte top, byte bottom, byte left, byte right)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceType, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        List<ActorBevBase> targetList = null;
        for (byte x = 0; x < this.m_GridComp.XGridCount; x++)
        {
            for (byte z = 0; z < this.m_GridComp.ZGridCount; z++)
            {
                List<ActorBevBase> tmpTargetList = new List<ActorBevBase>();
                foreach (var tmpTarget in findList)
                {
                    if (tmpTarget.GridData.XGrid == x
                        && (z - bottom) <= tmpTarget.GridData.ZGrid
                        && tmpTarget.GridData.ZGrid <= (z + top))
                    {
                        tmpTargetList.Add(tmpTarget);

                    }
                    else if (tmpTarget.GridData.ZGrid == z
                      && (x - left) <= tmpTarget.GridData.XGrid
                      && tmpTarget.GridData.XGrid <= (x + right))
                    {
                        tmpTargetList.Add(tmpTarget);
                    }
                }
                if (targetList == null || tmpTargetList.Count > targetList.Count)
                {
                    targetList = tmpTargetList;
                }
            }
        }
        return targetList;
    }

    /// <summary>
    /// 通过矩形的方式查找目标
    /// </summary>
    /// <param name="sourceList">源列表</param>
    /// <param name="width">长</param>
    /// <param name="height">宽</param>
    /// <returns>目标列表</returns>
    public List<ActorBevBase> FindTargetWithRect(List<ActorBevBase> sourceList, byte width, byte height)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceList[0].Type);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        List<ActorBevBase> totalTargetList = new List<ActorBevBase>(sourceList);
        foreach (var source in sourceList)
        {
            byte startX = (byte)Mathf.Max(0, source.GridData.XGrid - width + 1);
            byte startZ = (byte)Mathf.Max(0, source.GridData.ZGrid - height + 1);
            List<ActorBevBase> targetList = this.GetTargetListWithRect(findList, startX, source.GridData.XGrid, startZ, source.GridData.ZGrid, width, height);
            foreach (var target in targetList)
            {
                if (!totalTargetList.Contains(target))
                {
                    totalTargetList.Add(target);
                }
            }
        }
        return totalTargetList;
    }

    /// <summary>
    /// 通过十字形的方式查找目标
    /// </summary>
    /// <param name="sourceList">源列表</param>
    /// <param name="top">上</param>
    /// <param name="bottom">下</param>
    /// <param name="left">左</param>
    /// <param name="right">右</param>
    /// <returns>目标列表</returns>
    public List<ActorBevBase> FindTargetWithCross(List<ActorBevBase> sourceList, byte top, byte bottom, byte left, byte right)
    {
        List<ActorBevBase> findList = this.GetFindList(sourceList[0].Type);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }

        List<ActorBevBase> totalTargetList = new List<ActorBevBase>(sourceList);
        foreach (var source in sourceList)
        {
            foreach (var tmpTarget in findList)
            {
                if (tmpTarget.GridData.XGrid == source.GridData.XGrid
                    && (source.GridData.ZGrid - bottom) <= tmpTarget.GridData.ZGrid
                    && tmpTarget.GridData.ZGrid <= (source.GridData.ZGrid + top))
                {
                    if (!totalTargetList.Contains(tmpTarget))
                    {
                        totalTargetList.Add(tmpTarget);
                    }

                }
                else if (tmpTarget.GridData.ZGrid == source.GridData.ZGrid
                  && (source.GridData.XGrid - left) <= tmpTarget.GridData.XGrid
                  && tmpTarget.GridData.XGrid <= (source.GridData.XGrid + right))
                {
                    if (!totalTargetList.Contains(tmpTarget))
                    {
                        totalTargetList.Add(tmpTarget);
                    }
                }
            }
        }
        return totalTargetList;
    }

    /// <summary>
    /// 在指定范围内通过矩形的方式获得目标列表
    /// </summary>
    /// <param name="findList">查找列表</param>
    /// <param name="startX">起始X</param>
    /// <param name="endX">结束X</param>
    /// <param name="startZ">起始Z</param>
    /// <param name="endZ">结束Z</param>
    /// <param name="width">长</param>
    /// <param name="height">宽</param>
    /// <returns>目标列表</returns>
    private List<ActorBevBase> GetTargetListWithRect(List<ActorBevBase> findList, byte startX, byte endX, byte startZ, byte endZ, byte width, byte height)
    {
        List<ActorBevBase> targetList = null;
        for (byte x = startX; x <= endX; x++)
        {
            for (byte z = startZ; z <= endZ; z++)
            {
                List<ActorBevBase> tmpTargetList = new List<ActorBevBase>();
                foreach (var tmpTarget in findList)
                {
                    if (x <= tmpTarget.GridData.XGrid && tmpTarget.GridData.XGrid < x + width
                        && z <= tmpTarget.GridData.ZGrid && tmpTarget.GridData.ZGrid < z + height)
                    {
                        tmpTargetList.Add(tmpTarget);
                    }
                }
                if (targetList == null || tmpTargetList.Count > targetList.Count)
                {
                    targetList = tmpTargetList;
                }
            }
        }
        return targetList;
    }

    #endregion
}
