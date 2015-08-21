/// <summary>
/// 2级属性
/// </summary>
public enum L2AttributeType : byte
{
    /// <summary>
    /// 生命值
    /// </summary>
    HP = 0,

    /// <summary>
    /// 攻击力
    /// </summary>
    AP = 1,

    /// <summary>
    /// 攻击力
    /// </summary>
    PhysicsDEF = 2,

    /// <summary>
    /// 法术防御
    /// </summary>
    MagicDEf = 3,

    /// <summary>
    /// 速度
    /// </summary>
    Speed = 4,

    /// <summary>
    /// 命中
    /// </summary>
    Hit = 5,

    /// <summary>
    /// 闪避
    /// </summary>
    Dodge = 6,

    /// <summary>
    /// 暴击
    /// </summary>
    Critical = 7,

    /// <summary>
    /// 抗暴
    /// </summary>
    OpposeCritical = 8,

    /// <summary>
    /// 暴击伤害
    /// </summary>
    CriticalDamge = 9,

    /// <summary>
    /// 暴击伤害减免
    /// </summary>
    CriticalDamgeCounteract = 10,

    /// <summary>
    /// 治疗
    /// </summary>
    Heal = 11,

    /// <summary>
    /// 被治疗
    /// </summary>
    BeHealed = 12,

    /// <summary>
    /// 格挡
    /// </summary>
    Block = 13,

    /// <summary>
    /// 破挡
    /// </summary>
    Broken = 14,

    /// <summary>
    /// 特殊属性
    /// 七杀 怒气值
    /// 贪狼 法力值
    /// 破军 护盾值
    /// </summary>
    SpecialAttribute = 15,
}
