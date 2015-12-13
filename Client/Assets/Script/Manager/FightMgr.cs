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

    private List<ActorBevBase> GetFindList(ActorType sourceType, EffectTargetType effectTarget)
    {
        List<ActorBevBase> findList = null;
        if ((sourceType == ActorType.Enemy && effectTarget == EffectTargetType.Rival)
            || (sourceType == ActorType.Player && effectTarget == EffectTargetType.Friend))
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
    /// 查找最近的目标列表
    /// </summary>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="effectTarget"></param>
    /// <returns></returns>
    public List<ActorBevBase> FindNear(ActorBevBase source, byte count, EffectTargetType effectTarget)
    {
        List<ActorBevBase> findList = this.GetFindList(source.Type, effectTarget);
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
            for (int i = 0; i < count; i++)
            {
                ActorBevBase target = null;
                for (int j = 0; j < findList.Count; j++)
                {
                    ActorBevBase tmpTarget = findList[j];
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
                            && Mathf.Abs(tmpTarget.GridData.XGrid - source.GridData.XGrid) <= Mathf.Abs(tmpTarget.GridData.XGrid - source.GridData.XGrid)
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
            for (int i = 0; i < count; i++)
            {
                ActorBevBase target = null;
                byte minDistance = byte.MaxValue;
                for (int j = 0; j < findList.Count; j++)
                {
                    ActorBevBase tmpTarget = findList[j];
                    byte distance = (byte)(source.GridData.CalculateDistanceMagnitude(tmpTarget.GridData));
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
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="effectTarget"></param>
    /// <returns></returns>
    public List<ActorBevBase> FindFar(ActorBevBase source, byte count, EffectTargetType effectTarget)
    {
        List<ActorBevBase> findList = this.GetFindList(source.Type, effectTarget);
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
            for (int i = 0; i < count; i++)
            {
                ActorBevBase target = null;
                for (int j = 0; j < findList.Count; j++)
                {
                    ActorBevBase tmpTarget = findList[j];
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
                            && Mathf.Abs(tmpTarget.GridData.XGrid - source.GridData.XGrid) <= Mathf.Abs(tmpTarget.GridData.XGrid - source.GridData.XGrid)
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
            for (int i = 0; i < count; i++)
            {
                ActorBevBase target = null;
                byte maxDistance = byte.MaxValue;
                for (int j = 0; j < findList.Count; j++)
                {
                    ActorBevBase tmpTarget = findList[j];
                    byte distance = (byte)(source.GridData.CalculateDistanceMagnitude(tmpTarget.GridData));
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
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="effectTarget"></param>
    /// <returns></returns>
    public List<ActorBevBase> FindMinAttribute(ActorBevBase source, byte count, EffectTargetType effectTarget,AttributeType attribute)
    {
        List<ActorBevBase> findList = this.GetFindList(source.Type, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (findList.Count <= count)
        {
            return findList;
        }
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        for (int i = 0; i < count; i++)
        {
            ActorBevBase target = null;
            for (int j = 0; j < findList.Count; j++)
            {
                ActorBevBase tmpTarget = findList[j];
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
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="effectTarget"></param>
    /// <returns></returns>
    public List<ActorBevBase> FindMaxAttribute(ActorBevBase source, byte count, EffectTargetType effectTarget, AttributeType attribute)
    {
        List<ActorBevBase> findList = this.GetFindList(source.Type, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (findList.Count <= count)
        {
            return findList;
        }
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        for (int i = 0; i < count; i++)
        {
            ActorBevBase target = null;
            for (int j = 0; j < findList.Count; j++)
            {
                ActorBevBase tmpTarget = findList[j];
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
    /// </summary>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="effectTarget"></param>
    /// <returns></returns>
    public ActorBevBase FindOpposite(ActorBevBase source)
    {
        List<ActorBevBase> findList = this.GetFindList(source.Type, EffectTargetType.Rival);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        ActorBevBase target = null;
        for (int j = 0; j < findList.Count; j++)
        {
            ActorBevBase tmpTarget = findList[j];
            if (tmpTarget.GridData.XGrid == source.GridData.XGrid
                && (target == null) ? true : tmpTarget.GridData.ZGrid < target.GridData.ZGrid)
            {
                target = tmpTarget;
            }
        }
        return target;
    }

    /// <summary>
    /// 查找随机目标列表
    /// </summary>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="effectTarget"></param>
    /// <returns></returns>
    public List<ActorBevBase> FindRandom(ActorBevBase source, byte count, EffectTargetType effectTarget)
    {
        List<ActorBevBase> findList = this.GetFindList(source.Type, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (findList.Count <= count)
        {
            return findList;
        }
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        for (int i = 0; i < count; i++)
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
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="effectTarget"></param>
    /// <returns></returns>
    public List<ActorBevBase> FindField(ActorBevBase source, byte count, EffectTargetType effectTarget, FieldType field, string fieldValue)
    {
        List<ActorBevBase> findList = this.GetFindList(source.Type, effectTarget);
        if (findList == null || findList.Count == 0)
        {
            return null;
        }
        if (findList.Count <= count)
        {
            return findList;
        }
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        for (int i = 0; i < count; i++)
        {
            ActorBevBase target = null;
            for (int j = 0; j < findList.Count; j++)
            {
                ActorBevBase tmpTarget = findList[j];
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

    #endregion
}
