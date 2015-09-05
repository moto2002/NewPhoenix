public class TriggerSkillData : SkillDataBase
{
    #region 触发类型
    public AttributeType? TriggeredByAttribute;//通过属性触发
    public TriggerFieldType? TriggeredByField;//通过字段触发
    public BuffType? TriggeredByBuff;//通过Buff触发
    public FightActionType? TriggeredByFightBev;//通过战斗中的行为触发
    public CountingType? TriggeredByCounting;//通过计数触发
    #endregion
    public TriggerConditionType TriggerCondition;//触发类型
    public ActorType? CompareTarget;//比较类型
    public RateOrValueType? CompareRateOrVale;//通过百分百比较，或者通过值比较
    public float? CompareValue;//数据

    #region 触发的效果
    public int? TriggerSkill;//触发技能
    public int? TriggerBuff;//触发Buff
   
    #endregion


}
