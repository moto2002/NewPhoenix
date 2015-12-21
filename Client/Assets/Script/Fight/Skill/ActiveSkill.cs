using System.Linq;
using System.Collections.Generic;

public class ActiveSkill : SkillBase
{
    private ActiveSkillLogicData m_ActiveSkillLogicData;
    private ActiveSkillData m_ActiveSkillData;

    #region override methods

    public override void Init(ActorBevBase actor, SkillLogicDataBase logicData, SkillDataBase configData)
    {
        base.Init(actor, logicData, configData);
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
            return new List<ActorBevBase>() { this.Actor };

        if (this.m_ActiveSkillData.SkillRange == SkillRangeType.All)
            return FightMgr.Instance.FindAll(this.Actor.Type, this.m_ActiveSkillData.EffectTarget);

        List<ActorBevBase> selectionTargetList = new List<ActorBevBase>();
        switch (this.m_ActiveSkillData.SelectionTarget)
        {
            case SelectionTargetType.Near:
                selectionTargetList = FightMgr.Instance.FindNear(this.Actor.Type, this.Actor.GridData, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget);
                break;
            case SelectionTargetType.Far:
                selectionTargetList = FightMgr.Instance.FindFar(this.Actor.Type, this.Actor.GridData, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget);
                break;
            case SelectionTargetType.MinAttribute:
                selectionTargetList = FightMgr.Instance.FindMinAttribute(this.Actor.Type, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget, this.m_ActiveSkillData.SelectionTargetRefrenceAttribute.Value);
                break;
            case SelectionTargetType.MaxAttribute:
                selectionTargetList = FightMgr.Instance.FindMaxAttribute(this.Actor.Type, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget, this.m_ActiveSkillData.SelectionTargetRefrenceAttribute.Value);
                break;
            case SelectionTargetType.Max:
                //2015.12.14 02:28-log:写到这里
                switch (this.m_ActiveSkillData.SkillRange)
                {
                    case SkillRangeType.Rect:
                        return FightMgr.Instance.FindMaxRect(this.Actor.Type, this.m_ActiveSkillData.EffectTarget, this.m_ActiveSkillData.GetRectWidth(), this.m_ActiveSkillData.GetRectHeight());
                    case SkillRangeType.Cross:
                        return FightMgr.Instance.FindMaxCross(this.Actor.Type, this.m_ActiveSkillData.EffectTarget, this.m_ActiveSkillData.GetCrossTop(), this.m_ActiveSkillData.GetCrossBottom(), this.m_ActiveSkillData.GetCrossLeft(), this.m_ActiveSkillData.GetCrossRight());
                }
                break;
            case SelectionTargetType.Opposite:
                //2015.12.03 02:57-log:写到这里
                ActorBevBase target = FightMgr.Instance.FindOpposite(this.Actor.Type, this.Actor.GridData);
                selectionTargetList.Add(target);
                break;
            case SelectionTargetType.Random:
                selectionTargetList = FightMgr.Instance.FindRandom(this.Actor.Type, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget);
                break;
            case SelectionTargetType.Field:
                selectionTargetList = FightMgr.Instance.FindField(this.Actor.Type, this.m_ActiveSkillData.SelectionCount, this.m_ActiveSkillData.EffectTarget, this.m_ActiveSkillData.SelectionTargetRefrenceAttribute.Value, this.m_ActiveSkillData.SelectionTargetRefrenceFieldValue);
                break;
        }
        if (selectionTargetList == null || selectionTargetList.Count == 0)
            return null;
        //2015.12.14 02:32-log:写到这里
        if (this.m_ActiveSkillData.SkillRange == SkillRangeType.Single)
            return selectionTargetList;

        switch (this.m_ActiveSkillData.SkillRange)
        {
            case SkillRangeType.Rect:
                return FightMgr.Instance.FindTargetWithRect(selectionTargetList, this.m_ActiveSkillData.GetRectWidth(), this.m_ActiveSkillData.GetRectHeight());
            case SkillRangeType.Cross:
                return FightMgr.Instance.FindTargetWithCross(selectionTargetList, this.m_ActiveSkillData.GetCrossTop(), this.m_ActiveSkillData.GetCrossBottom(), this.m_ActiveSkillData.GetCrossLeft(), this.m_ActiveSkillData.GetCrossRight());
        }
        return null;
    }

    #endregion

}
