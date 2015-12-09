/// <summary>
/// 移动到目标
/// </summary>
public enum MoveToTargetType : byte
{
    /// <summary>
    /// 站在原地
    /// 不需要移动
    /// </summary>
    DontMove = 0,

    /// <summary>
    /// 只移动一次
    /// 移动到整体目标
    /// </summary>
    MoveToWhole = 1,

    /// <summary>
    /// 移动多次
    /// 移动到每个目标
    /// </summary>
    MoveToEveryOne = 2,
}
