/// <summary>
/// 触发条件
/// </summary>
public enum TriggerConditionType:byte
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
    /// 改变 
    /// 改变的值可正可负
    /// </summary>
    Change,
}
