public class TriggerSkillData : SkillDataBase
{
    public TriggerType TriggerType;//触发类型
    public byte TriggerValue;//触发的值
    public TriggerConditionType TriggerCondition;//触发条件
    public ActorType CompareTarget;//比较的对象
    public RateOrValueType? CompareRateOrVale;
    public float? CompareValue;//数据

    #region 触发的效果
    public int? TriggerSkill;//触发技能
    public int? TriggerBuff;//触发Buff
    #endregion
}
