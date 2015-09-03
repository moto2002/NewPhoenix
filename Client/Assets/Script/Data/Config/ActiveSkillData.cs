public class ActiveSkillData : SkillDataBase
{
    public SelectionTargetType SelectionTarget;//选择目标的方式
    public SkillRangeType SkillRange;//技能范围

    //当 SkillRange 为 Cross 时
    //eg:1,1,2,2 (上下左右)
    public byte CrossTop;
    public byte CrossBottom;
    public byte CrossLeft;
    public byte CrossRight;

    //当 SkillRange 为 Rect 时
    //eg:2,3
    public byte RectWidth;
    public byte RectHeight;

    //当 SkillRange 为 Point 时
    public byte TargetCount;//目标数量
    public byte CD;//CD回合数
    //消耗率 消耗值 消耗的东西根据英雄的职业而定
    public RateOrValueType CostRateOrValue;
    public float CostValue;//数据
    public int[] Buffs;//携带的BuffID

    public DamageType DamageType;//伤害类型
    public float Fluctuation;//波动

}
