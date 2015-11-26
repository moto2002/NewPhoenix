using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActorBevBase : MonoBehaviour
{
    public ActorType Type { get; protected set; }
    public Transform MyTransform { get; private set; }
    public GameObject MyGameObject { get; private set; }
    public ActorAIBase ActorAI { get; private set; }
    public ActorLogicData ActorLogicData { get; private set; }
    public byte Index { get; set; }
    public bool IsDead { get; private set; }
    public GridData GridData { get; private set; }

    #region event

    public event Action<WeatherType> ShowWeatherEvent;

    #endregion

    private bool m_IsMoves;//是否出招
    private float m_iTweenMoveTime = 0.2f;
    private Vector3 m_DefaultPosition;
    private SkillBase m_NormalAttack;
    private SkillBase m_CurSkill;
    private SkillBase m_WeatherSkill;
    private SkillBase m_FirstSkill;
    private List<SkillBase> m_SkillList;

    #region MonoBehaviour methods

    void Awake()
    {
        this.MyTransform = this.transform;
        this.MyGameObject = this.gameObject;
        this.SetActorType();

    }

    void Start()
    {

    }

    #endregion

    #region virtual methods

    protected virtual void SetActorType() { }

    #endregion

    #region protected methods

    protected void SetActorType(ActorType type)
    {
        this.Type = type;
    }

    #endregion

    #region public methods

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="data"></param>
    public void Init(ActorLogicData data)
    {
        this.ActorLogicData = data;
        this.m_DefaultPosition = this.MyTransform.position;

        this.ActorAI = this.MyGameObject.AddComponent<ActorAIBase>();
        this.ActorAI.InitStateMachine(this);

        foreach (var item0 in data.SkillList)
        {
            foreach (var item1 in item0.SkillList)
            {
                SkillBase skill = null;
                switch (item1.SkillType)
                {
                    case SkillType.Active:
                        skill = this.AddComponent<ActiveSkill>();
                        break;
                    case SkillType.Passive:
                        skill = this.AddComponent<PassiveSkill>();
                        break;
                    case SkillType.Trigger:
                        skill = this.AddComponent<TriggerSkill>();
                        break;
                    case SkillType.Buff:
                        skill = this.AddComponent<Buff>();
                        break;
                    case SkillType.Debuff:
                        skill = this.AddComponent<Debuff>();
                        break;
                    case SkillType.Weather:
                        skill = this.AddComponent<WeatherSkill>();
                        this.m_WeatherSkill = skill;
                        break;
                    case SkillType.First:
                        skill = this.AddComponent<ActiveSkill>();
                        this.m_FirstSkill = skill;
                        break;
                    default: continue;
                }
                skill.Init(this, item0, item1);
                if (!(item1.SkillType == SkillType.Weather
                    || item1.SkillType == SkillType.First
                    || item1.SkillType == SkillType.Normal))
                {
                    this.m_SkillList.Add(skill);
                }
            }
        }
        this.m_NormalAttack = this.AddComponent<NormalAttack>();
        this.m_NormalAttack.Init(this, data.GetNormalAttack());
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="data"></param>
    public void InitPlayer(string uid, GridData gridData)
    {
        this.Init(LogicCtrller.Instance.Actor.GetActorLogicDataByUID(uid));
        this.GridData = gridData;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="data"></param>
    public void InitEnemy(int id, GridData gridData)
    {
        ActorData actorData = ConfigCtrller.Instance.Actor.GetActorDataByID(id);
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
            switch (actorSkillData.SkillType)
            {
                case SkillType.Normal:
                    break;
                case SkillType.First:
                case SkillType.Active:
                    skillLogicData = new ActiveSkillLogicData(actorSkillData, skillDataBaseList);
                    break;
                case SkillType.Passive:
                    skillLogicData = new PassiveSkillLogicData(actorSkillData, skillDataBaseList);
                    break;
                case SkillType.Trigger:
                    skillLogicData = new TriggerSkillLogicData(actorSkillData, skillDataBaseList);
                    break;
                case SkillType.Weather:
                    skillLogicData = new WeatherSkillLogicData(actorSkillData, skillDataBaseList);
                    break;
                default:
                    break;
            }
            if (skillLogicData != null)
            {
                skillList.Add(skillLogicData);
            }
        }
        ActorLogicData actorLogicData = new ActorLogicData(actorData, skillList);
        this.Init(actorLogicData);
    }

    /// <summary>
    /// 是否出招
    /// </summary>
    /// <returns></returns>
    public bool IsMoves()
    {
        return this.m_IsMoves;
    }

    /// <summary>
    /// 出招
    /// </summary>
    public void Moves(ActorBevBase target, Vector3 movesPosition)
    {
        Debug.Log("准备出招");
        this.m_IsMoves = true;
        //这里需要做其他计算,得出是普通攻击还是技能或者其他什么什么的。。。
        this.MoveToTarget(movesPosition, true);
    }

    private void MoveToTarget(Vector3 position, bool goTo)
    {
        iTween.MoveTo(this.MyGameObject, iTween.Hash(iT.MoveTo.time, this.m_iTweenMoveTime,
            iT.MoveTo.position, position, iT.MoveTo.islocal, false,
            "ignoretimescale", false, iT.MoveTo.easetype, iTween.EaseType.linear,
          iT.MoveTo.oncomplete, "MoveComplete",
          iT.MoveAdd.oncompleteparams, goTo,
          iT.MoveTo.oncompletetarget, this.MyGameObject));
    }

    private void MoveComplete(bool goTo)
    {
        if (goTo)
        {
            Debug.Log("移动完成 攻击");
            this.ActorAI.Moves();
        }
        else
        {
            this.ActorMovesComplete();
        }
    }

    /// <summary>
    /// 清空出招
    /// </summary>
    public void ClearMoves()
    {
        this.m_IsMoves = false;
    }

    /// <summary>
    /// 下一回合
    /// </summary>
    public void NextRound()
    {
        this.ClearMoves();
    }

    /// <summary>
    /// 攻击完成
    /// </summary>
    public void AttackComplete()
    {
        Debug.Log("攻击完成 移动回来");
        this.ActorAI.AttackComplete();
        this.MoveToTarget(this.m_DefaultPosition, false);
    }

    /// <summary>
    /// 出招完成
    /// 出招分为:
    ///     1.移动过去
    ///     2.攻击
    ///     3.移动回来
    /// </summary>
    public void ActorMovesComplete()
    {
        Debug.Log("出招结束");
    }

    /// <summary>
    /// 攻击动画时间点
    /// </summary>
    private void Attacked()
    {

    }
    /// <summary>
    /// 蓄力动画时间点
    /// </summary>
    private void Charge() { }


    #endregion

    #region 出手

    public void Moves()
    {
        this.MovesStart();
    }

    /// <summary>
    /// 出手前：
    /// 增益buff
    /// 回合前结算的dot
    /// </summary>
    private void MovesStart()
    {
        this.Moving();
    }

    /// <summary>
    /// 出手中：
    /// 普通攻击
    /// 特殊技能
    /// 对手的响应技能
    /// </summary>
    private void Moving()
    {
        this.CheckSkill();
        this.m_CurSkill.Moves();
    }

    /// <summary>
    /// 出手后：
    /// 增益buff
    /// 回合后结算的dot
    /// 部分debuff消失的阶段
    /// </summary>
    private void MovesEnd()
    { }

    /// <summary>
    /// 检测是否要使用技能
    /// </summary>
    private void CheckSkill()
    {
        this.m_CurSkill = this.m_NormalAttack;

    }


    #endregion

    #region WeatherSkill

    public bool HasWeatherSkill
    {
        get
        {
            return this.m_WeatherSkill != null;
        }
    }

    public WeatherType? GetWeatherFromWeatherSkill
    {
        get
        {
            if (this.HasWeatherSkill)
            {
                WeatherSkillLogicData data = (WeatherSkillLogicData)this.ActorLogicData.GetWeatherSkill();
                return data.Weather;
            }
            return null;
        }
    }

    public void CastWeatherSkill()
    {
        if (this.HasWeatherSkill)
        {
            this.m_WeatherSkill.Moves();
        }
    }
    public void ShowWeather(WeatherType type)
    {
        if (this.HasWeatherSkill )
        {
            Debug.Log(this.ActorLogicData.Name + ": " + type.ToString());
            if (this.ShowWeatherEvent != null)
            {
                this.ShowWeatherEvent(type);
            }
        }
    }

    /// <summary>
    /// 移除天气技能
    /// </summary>
    public void RemoveWeatherSkill()
    {
        if (this.m_WeatherSkill != null)
        {
            Destroy(this.m_WeatherSkill);
        }
    }
    #endregion;

    #region FirstSkill

    public bool HasFirstSkill
    {
        get
        {
            return this.m_FirstSkill!=null;
        }
    }

    public void CastFirstSkill()
    {
        if (this.HasFirstSkill)
        {
            this.m_FirstSkill.Moves();
        }
    }

    /// <summary>
    /// 移除先机技能
    /// </summary>
    public void RemoveFirstSkill()
    {
        if (this.m_FirstSkill != null)
        {
            Destroy(this.m_FirstSkill);
        }
    }
    #endregion

   


}
