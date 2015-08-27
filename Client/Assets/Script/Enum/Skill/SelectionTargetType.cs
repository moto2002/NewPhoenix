
/// <summary>
/// 选择对象类型
/// </summary>
public enum SelectionTargetType : byte
{
    /// <summary>
    /// 就近（左下）
    /// </summary>
    Near,

    /// <summary>
    /// 最远
    /// </summary>
    Far,

    /// <summary>
    /// 某项属性最小
    /// </summary>
    MinAttribute,

    /// <summary>
    /// 某项属性最大
    /// </summary>
    MaxAttribute,

    /// <summary>
    /// 最多目标
    /// </summary>
    Max,

    /// <summary>
    /// 对面
    /// </summary>
    Opposite,

    /// <summary>
    /// 从后排开始打
    /// </summary>
    FromBack,

    /// <summary>
    /// 从前排开始
    /// </summary>
    FromForward,

    /// <summary>
    /// 随机
    /// </summary>
    Range,
}
