using System;
using System.Collections.Generic;

public class ActiveSkill : SkillBase
{
    private ActiveSkillLogicData m_ActiveSkillLogicData;
    private ActiveSkillData m_ActiveSkillData;

    #region override methods

    public override void Init(ActorBevBase actor,  SkillLogicDataBase logicData,SkillDataBase configData)
    {
        base.Init(actor,logicData,configData);
        this.m_ActiveSkillLogicData = (ActiveSkillLogicData)logicData;
        this.m_ActiveSkillData = (ActiveSkillData)configData;
    }

    public override void Moves()
    {
        List<ActorBevBase> targetList = this.FindTarget();
    }

    protected override List<ActorBevBase> FindTarget()
    {
        List<ActorBevBase> targetList = null;
        switch (this.m_ActiveSkillData.SelectionTarget)
        {
            case SelectionTargetType.Near:
                ActorBevBase target = FightMgr.Instance.FindNear(this.Actor, this.m_ActiveSkillData.EffectTarget);
                if (target != null)
                {
                    targetList.Add(target);
                }
                break;
            case SelectionTargetType.Far:

                break;
            case SelectionTargetType.MinAttribute:
                break;
            case SelectionTargetType.MaxAttribute:
                break;
            case SelectionTargetType.Max:
                break;
            case SelectionTargetType.Opposite:
                break;
            case SelectionTargetType.FromBack:
                break;
            case SelectionTargetType.FromForward:
                break;
            case SelectionTargetType.Random:
                break;
            case SelectionTargetType.Attribute:
                break;
            default:
                break;
        }
        return targetList;
    }

    #endregion


}
