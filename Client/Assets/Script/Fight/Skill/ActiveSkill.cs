using System.Linq;
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
        if (this.m_ActiveSkillData.EffectTarget == EffectTargetType.Self)
        {
            return new List<ActorBevBase>() { this.Actor };
        }
        else
        {
            List<ActorBevBase> selectionTargetList = new List<ActorBevBase>();
            switch (this.m_ActiveSkillData.SelectionTarget)
            {
                case SelectionTargetType.Near:
                    selectionTargetList = FightMgr.Instance.FindNear(this.Actor, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget);
                    break;
                case SelectionTargetType.Far:
                    selectionTargetList = FightMgr.Instance.FindFar(this.Actor, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget);
                    break;
                case SelectionTargetType.MinAttribute:
                    selectionTargetList = FightMgr.Instance.FindMinAttribute(this.Actor, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget, this.m_ActiveSkillData.SelectionTargetRefrenceAttribute.Value);
                    break;
                case SelectionTargetType.MaxAttribute:
                    selectionTargetList = FightMgr.Instance.FindMaxAttribute(this.Actor, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget, this.m_ActiveSkillData.SelectionTargetRefrenceAttribute.Value);
                    break;
                case SelectionTargetType.Max:
                    //2015.12.14 02:28-log:写到这里
                    break;
                case SelectionTargetType.Opposite:
                    //2015.12.03 02:57-log:写到这里
                    ActorBevBase target = FightMgr.Instance.FindOpposite(this.Actor);
                    break;
                case SelectionTargetType.Random:
                    selectionTargetList = FightMgr.Instance.FindRandom(this.Actor, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget);
                    break;
                case SelectionTargetType.Field:
                    selectionTargetList = FightMgr.Instance.FindField(this.Actor, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget, this.m_ActiveSkillData.SelectionTargetRefrenceField.Value, this.m_ActiveSkillData.SelectionTargetRefrenceFieldValue);
                    break;
            }
            //2015.12.14 02:32-log:写到这里
        }
    }

    #endregion


}
