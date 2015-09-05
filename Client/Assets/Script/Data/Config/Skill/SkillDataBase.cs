
public abstract class SkillDataBase
{
    public  int ID;//ID
    public string Name;//名称
    public string Description;//描述
    public string Icon;//图标
    public ActorType? EffectTarget;//作用于哪方

    //作用值，即效果
    public AttributeType? ChangeAttribute;//改变属性
    public RateOrValueType? ChangeRateOrVale;//百分百，或者值
    public float? ChangeValue;//数据

}
