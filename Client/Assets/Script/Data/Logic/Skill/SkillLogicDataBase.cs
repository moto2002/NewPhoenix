public abstract class SkillLogicDataBase
{
    private SkillDataBase m_Data;
    public SkillLogicDataBase(string uid, SkillDataBase data)
    {
        this.UID = uid;
        this.m_Data = data;
    }
    public string UID { get; private set; }
    public int ID { get { return this.m_Data.ID; } }
    public string Name { get { return this.m_Data.Name; } }
    public string Description { get { return this.m_Data.Description; } }
    public string Icon { get { return this.m_Data.Icon; } }
    public SkillType SkillType { get { return this.m_Data.SkillType; } }
    public ActorType? EffectTarget { get { return this.m_Data.EffectTarget; } }
    //作用值，即效果
    public AttributeType? ChangeAttribute { get { return this.m_Data.ChangeAttribute; } }//改变属性
    public RateOrValueType? ChangeRateOrVale { get { return this.m_Data.ChangeRateOrVale; } }//百分百，或者值
    public float? ChangeValue { get { return this.m_Data.ChangeValue; } }//数据

    public ShieldType? ShieldType { get { return this.m_Data.ShieldType; } }//护盾类型
}