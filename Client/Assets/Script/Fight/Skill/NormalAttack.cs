using UnityEngine;
using System.Collections.Generic;

public class NormalAttack : SkillBase
    {

    #region override methods

    public override void Init(ActorBevBase actor, SkillLogicDataBase data)
    {
        base.Init(actor, data);
    }

    protected override List<ActorBevBase> FindTarget()
    {
        ActorBevBase target = this.FindTarget(this.Actor.GridData, FightMgr.Instance.GetOtherSideActor(this.Actor.Type));
        if (target != null)
        {
            return new List<ActorBevBase>() { target };
        }

        return null;
    }


    public override void Moves()
    {
        List<ActorBevBase> targetList = this.FindTarget();
    }


    #endregion

    #region private methods

    private ActorBevBase FindTarget(GridData gridData, List<ActorBevBase> actorList)
    {
        byte minDistance = FightMgr.Instance.GetMaxMagnitudeDistance();
        byte minIndex = FightMgr.Instance.GetMaxIndex();
        ActorBevBase target = null;
        foreach (ActorBevBase actor in actorList)
        {
            byte distance = FightMgr.Instance.CalculateMagnitudeDistance(gridData, actor.GridData, true);
            if (distance < minDistance ||
               (distance == minDistance && actor.Index < minIndex))
            {
                target = actor;
                minIndex = target.Index;
                minDistance = distance;
            }
        }
        return target;
    }

    #endregion

}
