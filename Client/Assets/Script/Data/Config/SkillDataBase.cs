
public abstract class SkillDataBase
{
    public  int ID;//ID
    public string Name;//名称
    public string Description;//描述
    public ActorType EffectTarget;//作用于哪方
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

    //作用值
    public AttributeType AttributeType;//属性类型
    //可能是 造成的伤害率 或者 回复率
    //可能是 伤害值 或者 回复值
    // 根据英雄而定
    public RateOrValueType EffectRateOrValue;
    public float Effect;//数据

    public byte CD;//CD回合数
    //消耗率 消耗值 消耗的东西根据英雄的职业而定
    public RateOrValueType CostRateOrValue;
    public float Cost;//数据
    public int[] Buffs;//携带的BuffID
    public DamageType DamageType;//伤害类型


}
