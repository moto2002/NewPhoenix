/// <summary>
/// 触发条件
/// </summary>
public enum TriggerConditionType:byte
{ 
    /// <summary>
    /// 大于
    /// </summary>
    Greater = 0,

    /// <summary>
    /// 小于
    /// </summary>
    Less = 1,

    /// <summary>
    /// 等于
    /// </summary>
    Equal = 2,
   
    /// <summary>
    /// 改变 
    /// 改变的值可正可负
    /// </summary>
    Change = 3,
}
