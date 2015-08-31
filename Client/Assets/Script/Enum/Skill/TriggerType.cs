/// <summary>
/// 触发条件
/// </summary>
public enum TriggerType:byte
{ 
    /// <summary>
    /// 大于
    /// </summary>
    Greater,

    /// <summary>
    /// 小于
    /// </summary>
    Less,

    /// <summary>
    /// 等于
    /// </summary>
    Equal,
    
    /// <summary>
    /// 某项属性触发时
    /// eg：格挡
    /// </summary>
    Trigger,

    /// <summary>
    /// 改变 
    /// 改变的值可正可负
    /// </summary>
    Change,
}
