
public abstract class SkillDataBase
{
    public  int ID;//ID
    public string Name;//名称
    public string Description;//描述
    public ActorType UseTarget;//作用于哪方
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
    public AttributeType AttributeType;//属性类型

    //作用值
    public float DamageRate;//造成的伤害率
    public int DamageValue;//伤害值

    //触发
    public float TriggerRate;//触发率
    public int TriggerSkillID;//触发的技能
    public byte TriggerRoundCount;//触发作用的回合数

    public byte CD;//CD回合数
    public float CostRate;//消耗率
    public int CostValue;//消耗值
    public int BuffID;//准备设计Buff


}
