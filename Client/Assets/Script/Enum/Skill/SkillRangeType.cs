public enum SkillRangeType : byte
{
    /// <summary>
    /// 十字
    /// 填表格式 1,2,2,3(上下左右)
    /// </summary>
    Cross = 0,

    /// <summary>
    /// 矩形区域
    /// 填表格式 2,3
    /// </summary>
    Rect = 1,

    /// <summary>
    /// 单体
    /// </summary>
    Single = 2,

    /// <summary>
    /// 全体
    /// </summary>
    All = 3,

}
