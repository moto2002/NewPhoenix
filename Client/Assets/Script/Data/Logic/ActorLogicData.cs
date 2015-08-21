public class ActorLogicData
{
    private ActorData m_ActorData;

    public ActorLogicData(long uid, ActorData heroData)
    {
        this.UID = uid;
        this.m_ActorData = heroData;
    }

    #region public methods

    public float GetAttackSpeed()
    {
        return this.Speed;
    }

    public float GetFightingPower()
    {
        return 1;
    }

    #endregion

    public long UID { get; private set; }
    public int ID { get { return this.m_ActorData.ID; } }
    public string Name { get { return this.m_ActorData.Name; } }//名称
    public string Feature { get { return this.m_ActorData.Feature; } }//特征
    public string Description{ get { return this.m_ActorData.Description; } }//描述
    public string Icon { get { return this.m_ActorData.Icon; } }//图标
    public string Texture { get { return this.m_ActorData.Texture; } }//原画
    public string Model { get { return this.m_ActorData.Model; } }//模型
    public ProfessionType Profession { get { return this.m_ActorData.Profession; } }//职业
    public byte Quality{ get { return this.m_ActorData.Quality; } }//品质
    public NationalityType Nationality { get { return this.m_ActorData.Nationality; } }//国籍
    public ColorType Color { get { return this.m_ActorData.Color; } }//颜色
    #region 1级属性
    public L1AttributeType L1MainAttribute { get { return this.m_ActorData.L1MainAttribute; } }//1级主属性
    public int Power { get { return this.m_ActorData.Power; } }//力量
    public int IQ { get { return this.m_ActorData.IQ; } }//智力
    public int Agile { get { return this.m_ActorData.Agile; } }//敏捷
    public int Physique { get { return this.m_ActorData.Physique; } }//体质
    #endregion

    public WeaponType[] EnableWeaponTypes { get { return this.m_ActorData.EnableWeaponTypes; } }//能够装的武器类型
    public SkillData UniqueSkill { get { return this.m_ActorData.UniqueSkill; } }//绝技
    public SkillData[] Skills { get { return this.m_ActorData.Skills; } }//技能

    #region 2级属性
    public int HP { get { return this.m_ActorData.HP; } }//生命值
    public int AP { get { return this.m_ActorData.AP; } }//攻击力
    public int PhysicsDEF { get { return this.m_ActorData.PhysicsDEF; } }//物理防御
    public int MagicDEf { get { return this.m_ActorData.MagicDEf; } }//法术防御
    public int Speed { get { return this.m_ActorData.Speed; } }//速度
    public float Hit { get { return this.m_ActorData.Hit; } }//命中
    public float Dodge { get { return this.m_ActorData.Dodge; } }//闪避
    public float Critical { get { return this.m_ActorData.Critical; } }//暴击
    public float OpposeCritical { get { return this.m_ActorData.OpposeCritical; } }//抗暴
    public float CriticalDamge { get { return this.m_ActorData.CriticalDamge; } }//暴击伤害
    public float CriticalDamgeCounteract { get { return this.m_ActorData.CriticalDamgeCounteract; } }//暴击伤害减免
    public float Heal { get { return this.m_ActorData.Heal; } }//治疗
    public float BeHealed { get { return this.m_ActorData.BeHealed; } }//被治疗
    public float Block { get { return this.m_ActorData.Block; } }//格挡
    public float Broken { get { return this.m_ActorData.Broken; } }//破挡

    /// <summary>
    /// 特殊属性
    /// 七杀 怒气值
    /// 贪狼 法力值
    /// 破军 护盾值
    /// </summary>
    public int SpecialAttribute { get { return this.m_ActorData.SpecialAttribute; } }
    public ShieldType ShieldType { get { return this.m_ActorData.ShieldType; } }
    #endregion
}