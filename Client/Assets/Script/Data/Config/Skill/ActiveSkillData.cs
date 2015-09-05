public class ActiveSkillData : SkillDataBase
{
    public SelectionTargetType SelectionTarget;//选择目标的方式
    public SkillRangeType SkillRange;//技能范围

    //当 SkillRange 为 Cross 时
    //eg:1,1,2,2 (上下左右)
    public byte? CrossTop;
    public byte? CrossBottom;
    public byte? CrossLeft;
    public byte? CrossRight;

    //当 SkillRange 为 Rect 时
    //eg:2,3
    public byte? RectWidth;
    public byte? RectHeight;

    //当 SkillRange 为 Point 时
    public byte? TargetCount;//目标数量
    public byte CD;//CD回合数
    //消耗率 消耗值 消耗的东西根据英雄的职业而定
    public RateOrValueType CostRateOrValue;
    public float CostValue;//数据

    #region 触发的Buff
    public float? TriggerBuffProbability0;//触发Buff的概率
    public int? TriggerBuffID0;//触发的Buff
    public int? TriggerBuffContinueRoundCount0;//触发Buff的持续回合数

    public float? TriggerBuffProbability1;//触发Buff的概率
    public int? TriggerBuffID1;//触发的Buff
    public int? TriggerBuffContinueRoundCount1;//触发Buff的持续回合数
    #endregion
    public DamageType DamageType;//伤害类型
    public float Fluctuation;//波动

    public float? DamageRate;
    /// <summary>
    /// 回复生命 = 源（RestoreHPeSource） X 率（DamageRate）
    /// false(0)：表示施放方的攻击
    /// true(1)：表示作用放的血量
    /// </summary>
    public bool? RestoreHPeSource;
}
