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
        //2015.12.01-log:需要明确十字形范围 是属于那种选择类型
        List<ActorBevBase> targetList = new List<ActorBevBase>();
        switch (this.m_ActiveSkillData.SelectionTarget)
        {
            case SelectionTargetType.Near:
                ActorBevBase nearTarget = FightMgr.Instance.FindNear(this.Actor, this.m_ActiveSkillData.EffectTarget);
                if (nearTarget != null)
                {
                    targetList.Add(nearTarget);
                }
                break;
            case SelectionTargetType.Far:
                ActorBevBase farTarget = FightMgr.Instance.FindFar(this.Actor, this.m_ActiveSkillData.EffectTarget);
                if (farTarget != null)
                {
                    targetList.Add(farTarget);
                }
                break;
            case SelectionTargetType.MinAttribute:
                ActorBevBase minAttributeTarget = FightMgr.Instance.FindMinAttribute(this.Actor, this.m_ActiveSkillData.EffectTarget, this.m_ActiveSkillData.SelectionTargetRefrenceAttribute.Value);
                if (minAttributeTarget != null)
                {
                    targetList.Add(minAttributeTarget);
                }
                break;
            case SelectionTargetType.MaxAttribute:
                ActorBevBase maxAttributeTarget = FightMgr.Instance.FindMaxAttribute(this.Actor, this.m_ActiveSkillData.EffectTarget, this.m_ActiveSkillData.SelectionTargetRefrenceAttribute.Value);
                if (maxAttributeTarget != null)
                {
                    targetList.Add(maxAttributeTarget);
                }
                break;
            case SelectionTargetType.Max:
                byte width = this.m_ActiveSkillData.GetRectWidth();
                byte height = this.m_ActiveSkillData.GetRectHeight();
                if (width == 0 || height == 0)return null;
                targetList = FightMgr.Instance.FindMaxCount(this.Actor, this.m_ActiveSkillData.EffectTarget,width,height );
                break;
            case SelectionTargetType.Opposite:
                //2015.12.03 02:57-log:写到这里
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
