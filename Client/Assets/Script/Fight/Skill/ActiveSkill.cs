using System;
using System.Collections.Generic;

public class ActiveSkill : SkillBase
{
    #region override methods

    public override void Init(ActorBevBase actor, SkillLogicDataBase data)
    {
        base.Init(actor, data);
    }

    public override void Moves()
    {
        throw new NotImplementedException();
    }

    protected override List<ActorBevBase> FindTarget()
    {
        throw new NotImplementedException();
    }

    #endregion


}
