public class ActiveSkillData : SkillDataBase
{
    public SelectionTargetType SelectionTarget;//选择目标的方式
    public AttributeType? SelectionTargetRefrenceAttribute;//选择目标参考属性
    public SkillRangeType SkillRange;//技能范围

    //当 SkillRange 为 Cross 时
    //eg:1,1,2,2 (上下左右)
    //当 SkillRange 为 Rect 时
    //eg:2,3
    //当 SkillRange 为 Point 时
    public byte[] RangeValue;

    public byte CD;//CD回合数
    //消耗率 消耗值 消耗的东西根据英雄的职业而定
    public RateOrValueType CostRateOrValue;
    public float CostValue;//数据
    public int[] TriggerBuffs;//触发的Buff
    public DamageType DamageType;//伤害类型
    public float Fluctuation;//波动
    public float? DamageRate;//造成的伤害率
    /// <summary>
    /// 回复生命 = 源（RestoreHPeSource） X 率（DamageRate）
    /// false(0)：表示施放方的攻击
    /// true(1)：表示作用放的血量
    /// </summary>
    public bool? RestoreHPeSource;
}
