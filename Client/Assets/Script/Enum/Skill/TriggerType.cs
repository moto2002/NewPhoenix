/// <summary>
/// 触发类型
/// </summary>
public enum TriggerType : byte
{
    /// <summary>
    /// 通过属性触发
    /// 参考枚举：AttributeType
    /// </summary>
    Attribute = 0,

    /// <summary>
    /// 通过字段触发
    /// 参考枚举：TriggerFieldType
    /// </summary>
    Field = 1,

    /// <summary>
    /// 通过Buff触发
    /// 参考枚举：BuffType
    /// </summary>
    Buff = 2,

    /// <summary>
    /// 通过战斗中的行为触发
    /// 参考枚举：FightActionType
    /// </summary>
    FightAction = 3,

    /// <summary>
    /// 通过计数触发
    /// 参考枚举：CountingType
    /// </summary>
    Counting = 4
}
