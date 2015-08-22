﻿public class ActorLogicData
{
    public long UID { get; private set; }
    public int ID { get { return this.m_ActorData.ID; } }
    public string Name { get { return this.m_ActorData.Name; } }
    public string Description { get { return this.m_ActorData.Description; } }
    public string Icon { get { return this.m_ActorData.Icon; } }
    public string AssetName { get { return this.m_ActorData.AssetName; } }
    public float BaseAttackSpeed { get { return this.m_ActorData.BaseAttackSpeed; } }

    private ActorData m_ActorData;

    public ActorLogicData(long uid, ActorData heroData)
    {
        this.UID = uid;
        this.m_ActorData = heroData;
    }

    #region public methods

    public float GetAttackSpeed()
    {
        return this.BaseAttackSpeed;
    }

    public float GetFightingPower()
    {
        return 1;
    }

    #endregion

    #region 新的字段

    public int ID_ { get { return this.m_ActorData.ID_; } }//ID
    public int Name_;//名称
    public int Feature;//特征
    public ProfessionType Profession;//职业
    public byte Quality;//品质
    public NationalityType Nationality;//国籍
    public SexType Sex;//性别
    public string Icon_;//头像
    public string Model;//模型
    public string Texture;//原画
    #region 1级属性
    public L1AttributeType L1MainAttribute;//1级主属性
    public int Power;//力量
    public int IQ;//智力
    public int Agile;//敏捷
    public int Physique;//体质
    #endregion

    public WeaponType[] EnableWeaponTypes;//能够装的武器类型
    public SkillData UniqueSkill;//绝技
    public SkillData[] Skills;//技能

    #region 2级属性
    public int HP;//生命值
    public int AP;//攻击力
    public int PhysicsDEF;//物理防御
    public int MagicDEf;//法术防御
    public int Speed;//速度
    public float Hit;//命中
    public float Dodge;//闪避
    public float Critical;//暴击
    public float OpposeCritical;//抗暴
    public float CriticalDamge;//暴击伤害
    public float CriticalDamgeCounteract;//暴击伤害减免
    public float Heal;//治疗
    public float BeHealed;//被治疗
    public float Block;//格挡
    public float Broken;//破挡

    /// <summary>
    /// 特殊属性
    /// 七杀 怒气值
    /// 贪狼 法力值
    /// 破军 护盾值
    /// </summary>
    public int SpecialAttribute;
    public ShieldType ShieldType;
    #endregion

    #endregion


}