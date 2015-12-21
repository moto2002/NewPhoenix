using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorLogicData
{
    private ActorData m_ActorData;
    private List<SkillLogicDataBase> m_SkillList;

    public ActorLogicData(ActorData actorData, List<SkillLogicDataBase> skillList)
    {
        this.m_ActorData = actorData;
        this.m_SkillList = skillList;
    }

    public ActorLogicData(string uid, ActorData actorData, List<SkillLogicDataBase> skillList)
    {
        this.UID = uid;
        this.m_ActorData = actorData;
        this.m_SkillList = skillList;
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

    #region Skill

    public SkillLogicDataBase GetNormalAttack()
    {
        return this.GetSkillByType(SkillType.Normal);
    }
    public SkillLogicDataBase GetWeatherSkill()
    {
        if (this.HasWeatherSkill)
        {
            return this.m_SkillList.First(a => a.SkillType == SkillType.Normal);
        }
        return null;
    }
    public SkillLogicDataBase GetFirstSkill()
    {
        if (this.HasFirstSkill)
        {
            return this.m_SkillList.First(a => a.SkillType == SkillType.Normal);
        }
        return null;
    }
    private SkillLogicDataBase GetSkillByType(SkillType type)
    {
        return this.m_SkillList.FirstOrDefault(a => a.SkillType == type);
    }

    public bool HasWeatherSkill
    {
        get
        {
            return this.m_SkillList.Any(a => a.SkillType == SkillType.Weather);
        }
    }

    public bool HasFirstSkill
    {
        get
        {
            return this.m_SkillList.Any(a => a.SkillType == SkillType.First);
        }
    }

    public List<SkillLogicDataBase> SkillList
    {
        get
        {
            return this.m_SkillList;
        }
    }
    #endregion

    #endregion

    public string UID { get; private set; }
    public int ID { get { return this.m_ActorData.ID; } }
    public string Name { get { return this.m_ActorData.Name; } }//名称
    public string Description { get { return this.m_ActorData.Description; } }//描述
    public string Icon { get { return this.m_ActorData.Icon; } }//图标
    public string Texture { get { return this.m_ActorData.Texture; } }//原画
    public string Model { get { return this.m_ActorData.Model; } }//模型
    public ProfessionType? Profession { get { return this.m_ActorData.Profession; } }//职业
    public NationalityType? Nationality { get { return this.m_ActorData.Nationality; } }//国籍
    public ColorType Color { get { return this.m_ActorData.Color; } }//颜色
    public SexType? Sex { get { return this.m_ActorData.Sex; } }//性别
    public byte Quality { get { return this.m_ActorData.Quality; } }//品质
    public byte LV { get { return this.m_ActorData.LV; } }//等级

    #region 1级属性
    public AttributeType? L1MainAttribute { get { return this.m_ActorData.L1MainAttribute; } }//1级主属性
    public int Power { get { return this.m_ActorData.Power; } }//力量
    public int IQ { get { return this.m_ActorData.IQ; } }//智力
    public int Agile { get { return this.m_ActorData.Agile; } }//敏捷
    public int Physique { get { return this.m_ActorData.Physique; } }//体质
    #endregion

    public WeaponType[] EnableWeaponTypes { get { return this.m_ActorData.EnableWeaponTypes; } }//能够装的武器类型
    //public SkillDataBase UniqueSkill { get { return this.m_ActorData.UniqueSkill; } }//绝技
    //public SkillDataBase[] Skills { get { return this.m_ActorData.Skills; } }//技能

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
    #endregion

    public float GetAttrbuteValueByType(AttributeType type)
    {
        switch (type)
        {
            case AttributeType.Power: return this.Power;
            case AttributeType.IQ: return this.IQ;
            case AttributeType.Agile: return this.Agile;
            case AttributeType.Physique: return this.Physique;
            case AttributeType.HP: return this.HP;
            case AttributeType.AP: return this.AP;
            case AttributeType.PhysicsDEF: return this.PhysicsDEF;
            case AttributeType.MagicDEf: return this.MagicDEf;
            case AttributeType.Speed: return this.Speed;
            case AttributeType.Hit: return this.Hit;
            case AttributeType.Dodge: return this.Dodge;
            case AttributeType.Critical: return this.Critical;
            case AttributeType.OpposeCritical: return this.OpposeCritical;
            case AttributeType.CriticalDamge: return this.CriticalDamge;
            case AttributeType.CriticalDamgeCounteract: return this.CriticalDamgeCounteract;
            case AttributeType.Heal: return this.Heal;
            case AttributeType.BeHealed: return this.BeHealed;
            case AttributeType.Block: return this.Block;
            case AttributeType.Broken: return this.Broken;
            case AttributeType.SpecialAttribute: return this.SpecialAttribute;
        }
        Debug.LogError("AttributeType:" + type + "类型错误");
        return float.MinValue;
    }

    public string GetFieldValueByType(AttributeType type)
    {
        switch (type)
        {
            case AttributeType.ID: return this.ID.ToString();
            case AttributeType.Name: return this.Name;
            case AttributeType.Profession:
                if (this.Profession.HasValue)
                {
                    return this.Profession.Value.ToString();
                }
                break;
            case AttributeType.Nationality:
                if (this.Nationality.HasValue)
                {
                    return this.Nationality.Value.ToString();
                }
                break;
            case AttributeType.Color: return Color.ToString();
            case AttributeType.Sex:
                if (this.Sex.HasValue)
                {
                    return this.Sex.ToString();
                }
                break;
            case AttributeType.Quality: return this.Quality.ToString();
            case AttributeType.LV: return this.LV.ToString();
        }
        Debug.LogError("FieldType:" + type + "类型错误");
        return null;
    }
}