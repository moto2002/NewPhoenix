
/// <summary>
/// 选择对象类型
/// </summary>
public enum SelectionTargetType : byte
{
    /// <summary>
    /// 就近（左下）
    /// </summary>
    Near = 0,

    /// <summary>
    /// 最远
    /// </summary>
    Far = 1,

    /// <summary>
    /// 某项属性最小
    /// </summary>
    MinAttribute = 2,

    /// <summary>
    /// 某项属性最大
    /// </summary>
    MaxAttribute = 3,

    /// <summary>
    /// 最多目标
    /// </summary>
    Max = 4,

    /// <summary>
    /// 对面
    /// </summary>
    Opposite = 5,

    /// <summary>
    /// 从后排开始打
    /// </summary>
    FromBack = 6,

    /// <summary>
    /// 从前排开始
    /// </summary>
    FromForward = 7,

    /// <summary>
    /// 随机
    /// </summary>
    Random = 8,

    /// <summary>
    /// 根据某项属性筛选
    /// 例如：国籍，职业等
    /// </summary>
    Attribute = 9,
}
