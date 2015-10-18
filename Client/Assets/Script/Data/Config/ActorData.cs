public class ActorData
{
    public int ID;//ID
    public string Name;//名称
    public string Description;//描述
    public string Icon;//图标
    public string Texture;//原画
    public string Model;//模型
    public ProfessionType? Profession;//职业
    public NationalityType? Nationality;//国籍
    public ColorType Color;//颜色
    public SexType? Sex;//性别
    public byte Quality;//品质
    public byte LV;//品质
    #region 1级属性
    public AttributeType? L1MainAttribute;//1级主属性
    public int Power;//力量
    public int IQ;//智力
    public int Agile;//敏捷
    public int Physique;//体质
    #endregion

    public WeaponType[] EnableWeaponTypes;//能够装的武器类型
    public int[] Skills;//技能

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

    //不要
    public ShieldType? ShieldType;//护盾类型

    #endregion
}
