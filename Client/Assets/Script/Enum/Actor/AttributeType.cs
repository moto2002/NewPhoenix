/// <summary>
/// 属性
/// </summary>
public enum L1AttributeType : byte
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
    /// 攻击力
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

}
