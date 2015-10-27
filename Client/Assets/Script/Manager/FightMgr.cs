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
    public Dictionary<ActorBevBase, GridData> EnemyBattleArray { get; private set; }//敌人阵容
    public Dictionary<ActorBevBase, GridData> PlayerBattleArray { get; private set; }//玩家阵容

    private FightGridComponent m_GridComp;//网格
    private byte m_RandCount;//回合数
    private bool m_GameOver;//是否游戏结束
    private ActorBevBase m_CurMovesActor;

    #region MonoBehaviour methods

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        this.Init();
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
        ActorData actorData = LogicController.Instance.Actor.GetActorDataByID(id);
        GameObject actor = PoolMgr.Instance.GetModel(AssetPathConst.Actor + actorData.Model);
        TransUtils.ChangeLayer(actor.transform, LayerConst.Actor);
        T bev = actor.AddComponent<T>();
        bev.Index = index;
        return bev;
    }
    /// <summary>
    /// 获取下一个出招的角色
    /// </summary>
    /// <returns></returns>
    private ActorBevBase GetNextMovesActor()
    {
        ActorBevBase player = this.GetNextMovesActor(this.PlayerBattleArray.Keys.ToList());
        ActorBevBase enemy = this.GetNextMovesActor(this.EnemyBattleArray.Keys.ToList());
        return this.GetNextMovesActor(player, enemy);
    }

    /// <summary>
    /// 比较玩家和敌人,得到下一个出招的角色
    /// </summary>
    /// <returns></returns>
    private ActorBevBase GetNextMovesActor(ActorBevBase player, ActorBevBase enemy)
    {
        if (player == null) return enemy;
        if (enemy == null) return player;
        List<ActorBevBase> actorList = new List<ActorBevBase>() { player, enemy };
        return this.GetNextMovesActor(actorList);
    }

    /// <summary>
    /// 计算玩家或角色得到最符合条件的下一个出招的角色
    /// </summary>
    /// <returns></returns>
    private ActorBevBase GetNextMovesActor<T>(List<T> actorList) where T : ActorBevBase
    {
        List<T> list = actorList.Where(a => !(a.IsDead || a.IsMoves()))
            .OrderByDescending(b => b.ActorLogicData.GetAttackSpeed())
            .ThenByDescending(c => c.ActorLogicData.GetFightingPower())
            .ToList();
        return list.FirstOrDefault();
    }

    /// <summary>
    /// 重置敌人和玩家
    /// </summary>
    private void ResetActor()
    {
        this.ResetActor(this.EnemyBattleArray.Keys.ToList());
        this.ResetActor(this.PlayerBattleArray.Keys.ToList());
        this.m_CurMovesActor = null;
    }

    /// <summary>
    /// 重置角色的状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="actorList"></param>
    private void ResetActor<T>(List<T> actorList) where T : ActorBevBase
    {
        actorList.ForEach(a =>
        {
            if (!a.IsDead)
            {
                a.NextRound();
            }
        });
    }

    private ActorBevBase GetTarget(ActorBevBase actorBev)
    {
        switch (actorBev.Type)
        {
            case ActorType.Enemy:
                return this.GetTarget(this.EnemyBattleArray[actorBev], this.PlayerBattleArray);
            case ActorType.Player:
                return this.GetTarget(this.PlayerBattleArray[actorBev], this.EnemyBattleArray);
        }
        return null;
    }

    private ActorBevBase GetTarget(GridData gridData, Dictionary<ActorBevBase, GridData> targetDic)
    {
        byte minDistance = this.m_GridComp.GetMaxMagnitudeDistance();
        byte minIndex = this.m_GridComp.GetMaxIndex();
        ActorBevBase target = null;
        foreach (KeyValuePair<ActorBevBase, GridData> kv in targetDic)
        {
            ActorBevBase acotrBev = kv.Key;
            byte distance = this.m_GridComp.CalculateMagnitudeDistance(gridData, kv.Value, true);
            if (distance < minDistance ||
               (distance == minDistance && acotrBev.Index < minIndex))
            {
                //Debug.Log("minDistance " + minDistance+ " distance " + distance+ "  minIndex " + minIndex + " acotrBev.Index " + acotrBev.Index +"  actorName "+ acotrBev.name);
                minIndex = acotrBev.Index;
                minDistance = distance;
                target = acotrBev;
            }
        }
        return target;
    }

    private void ActorMoves(ActorBevBase actorBev)
    {
        Debug.Log("出招 " + actorBev.name);
        ActorBevBase target = this.GetTarget(actorBev);
        if (target == null)
        {
            Debug.LogError("Target is null");
            return;
        }
        Vector3 position = this.m_GridComp.GetMovesPosition(target);
        actorBev.Moves(target, position);
    }

    #endregion

    #region public methods
    #endregion

    #region 游戏流程
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        this.InitScene();
        this.InitPlayer();
        this.InitEnemy();
        this.InitUI();
    }
    /// <summary>
    /// 初始化玩家
    /// </summary>
    private void InitPlayer()
    {
        this.PlayerBattleArray = new Dictionary<ActorBevBase, GridData>();
        foreach (KeyValuePair<byte, long> kv in LogicController.Instance.Actor.GetBattleArray())
        {
            byte index = kv.Key;
            long uid = kv.Value;
            GridData gridData = this.m_GridComp.ConvertIndexToGridData(index);
            PlayerBev playerBev = this.InstantiateActor<PlayerBev>(index, LogicController.Instance.Actor.GetActorLogicDataByUID(uid).ID);
            playerBev.transform.SetParent(this.PlayerParent);
            playerBev.transform.position = this.m_GridComp.ConvertGridToPosition(gridData, ActorType.Player);
            playerBev.transform.localRotation = Quaternion.identity;
            playerBev.transform.localScale = Vector3.one;
            playerBev.InitPlayer(uid);
            this.PlayerBattleArray.Add(playerBev, gridData);
        }
    }

    /// <summary>
    /// 初始化敌人
    /// </summary>
    private void InitEnemy()
    {
        this.EnemyBattleArray = new Dictionary<ActorBevBase, GridData>();
        LevelLogicData levelLogicData = LogicController.Instance.Level.GetLevelLogicDataByID(1000);
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
            enemyBev.InitEnemy(id);
            this.EnemyBattleArray.Add(enemyBev, gridData);
        }
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
        UIController.Instance.OpenPanel(UIPanelType.FightPanel);
    }
    /// <summary>
    /// 创建天气
    /// </summary>
    private void CreateWeather()
    { }
    /// <summary>
    /// 先机技能
    /// </summary>
    private void FirstMoves()
    {
    }
    /// <summary>
    /// 回合中
    /// </summary>
    private void Rounding()
    {
        if (this.SearchMovesActor())
        {
            this.RoundEnd();
        }
        else
        {
            this.ActorMovesStart();
            this.ActorMovesing();
            this.ActorMovesEnd();
        }
    }
    /// <summary>
    /// 角色出手开始
    /// </summary>
    private void ActorMovesStart()
    { }
    /// <summary>
    /// 角色出手中
    /// </summary>
    private void ActorMovesing()
    { }
    /// <summary>
    /// 角色出手结束
    /// </summary>
    private void ActorMovesEnd()
    { }

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
            //回合数加一 
            this.m_RandCount++;
            //重置参数
            this.ResetActor();
            //进入下一回合
            this.Rounding();
        }
    }
    /// <summary>
    /// 搜索出手角色
    /// </summary>
    /// <returns></returns>
    private bool SearchMovesActor()
    {
        return false;
    }
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
    { }
    /// <summary>
    /// 失败
    /// </summary>
    private void Fail()
    { }
    #endregion 游戏流程
}
