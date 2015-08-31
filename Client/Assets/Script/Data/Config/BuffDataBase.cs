public class BuffDataBase
{
    public int ID;//ID
    public string Name;//名称
    public string Description;//描述

    //自身
    public AttributeType EffectAttributeType;//改变的属性类型
    //作用值
    //可正可负
    public RateOrValueType EffectRateOrValue;
    public float Effect;//数据

    //被攻击
    public DamageType? DamageType;//改变被攻击属性的类型
    public RateOrValueType DamageEffectRateOrValue;
    //作用值
    //可正可负
    public float DamageEffect;//数据

}
