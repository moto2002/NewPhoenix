using UnityEngine;
using System.Collections.Generic;

public abstract class SkillBase : MonoBehaviour
{
    public ActorBevBase Actor { get;private set; }
    /// <summary>
    /// 查找目标
    /// </summary>
    /// <returns></returns>
    protected abstract List<ActorBevBase> FindTarget();

    /// <summary>
    /// 使用技能出招
    /// </summary>
    public abstract void Moves();

    public virtual void Init(ActorBevBase actor, SkillDataBase data)
    {
        this.Actor = actor;
    }
    public virtual void Init(ActorBevBase actor, SkillLogicDataBase data)
    {
        this.Actor = actor;
    }
    public virtual void Init(ActorBevBase actor, SkillLogicDataBase logicData, SkillDataBase configData)
    {
        this.Actor = actor;
    }
}
