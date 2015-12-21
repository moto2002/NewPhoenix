/// <summary>
/// 属性
/// </summary>
public enum AttributeType : byte
{
    #region 1级属性
    /// <summary>
    /// 力量
    /// </summary>
    Power = 0,

    /// <summary>
    /// 智力
    /// </summary>
    IQ = 1,

    /// <summary>
    /// 敏捷
    /// </summary>
    Agile = 2,

    /// <summary>
    /// 体质
    /// </summary>
    Physique = 3,

    #endregion

    #region 2级属性
    /// <summary>
    /// 生命值
    /// </summary>
    HP = 4,

    /// <summary>
    /// 攻击力
    /// </summary>
    AP = 5,

    /// <summary>
    /// 物理防御
    /// </summary>
    PhysicsDEF = 6,

    /// <summary>
    /// 法术防御
    /// </summary>
    MagicDEf = 7,

    /// <summary>
    /// 速度
    /// </summary>
    Speed = 8,

    /// <summary>
    /// 命中
    /// </summary>
    Hit = 9,

    /// <summary>
    /// 闪避
    /// </summary>
    Dodge = 10,

    /// <summary>
    /// 暴击
    /// </summary>
    Critical = 11,

    /// <summary>
    /// 抗暴
    /// </summary>
    OpposeCritical = 12,

    /// <summary>
    /// 暴击伤害
    /// </summary>
    CriticalDamge = 13,

    /// <summary>
    /// 暴击伤害减免
    /// </summary>
    CriticalDamgeCounteract = 14,

    /// <summary>
    /// 治疗
    /// </summary>
    Heal = 15,

    /// <summary>
    /// 被治疗
    /// </summary>
    BeHealed = 16,

    /// <summary>
    /// 格挡
    /// </summary>
    Block = 17,

    /// <summary>
    /// 破挡
    /// </summary>
    Broken = 18,

    /// <summary>
    /// 特殊属性
    /// 七杀 怒气值
    /// 贪狼 法力值
    /// 破军 护盾值
    /// </summary>
    SpecialAttribute = 19,

    #endregion

    #region 隐藏属性
    /// <summary>
    /// 最终伤害值
    /// </summary>
    FinalDamage = 20,

    /// <summary>
    /// 最终减免值
    /// </summary>
    FinalDamgeCounteract = 21,

    /// <summary>
    /// 最终伤害加成
    /// </summary>
    FinalDamageAddition = 22,

    /// <summary>
    /// 最终减免加成
    /// </summary>
    FinalDamageCounteractAddition =  23,

    /// <summary>
    /// 物理伤害加成
    /// </summary>
    PhysicsDamageAddition = 24,

    /// <summary>
    /// 物理减免加成
    /// </summary>
    PhysicsDamageCounteractAddition = 25,

    /// <summary>
    /// 法术伤害加成
    /// </summary>
    MagicDamageAddition = 26,

    /// <summary>
    /// 法术减免加成
    /// </summary>
    MagicDamageCounteractAddition = 27,
    #endregion

    #region 字段

    ID = 28,

    /// <summary>
    /// 名字
    /// </summary>
    Name = 29,

    /// <summary>
    /// 职业
    /// </summary>
    Profession = 30,

    /// <summary>
    /// 国籍
    /// </summary>
    Nationality = 31,

    /// <summary>
    /// 颜色
    /// </summary>
    Color = 32,

    /// <summary>
    /// 性别
    /// </summary>
    Sex =33,

    /// <summary>
    /// 品质
    /// </summary>
    Quality = 34,

    /// <summary>
    /// 等级
    /// </summary>
    LV = 35,
    #endregion

}
