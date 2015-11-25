/// <summary>
/// 技能类型
/// </summary>
public enum SkillType : byte
{
    /// <summary>
    /// 普通攻击
    /// </summary>
    Normal = 0,

    /// <summary>
    /// 主动
    /// </summary>
    Active = 1,

    /// <summary>
    /// 被动
    /// </summary>
    Passive = 2,

    /// <summary>
    /// 触发
    /// </summary>
    Trigger = 3,

    /// <summary>
    /// 天气技能
    /// </summary>
    Weather = 4,

    /// <summary>
    /// 先机技能
    /// </summary>
    First = 5,

    Buff = 6,
    Debuff = 7,
}
