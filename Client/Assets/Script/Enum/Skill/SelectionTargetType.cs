
/// <summary>
/// 选择对象类型
/// </summary>
public enum SelectionTargetType : byte
{
    /// <summary>
    /// 就近（左下）
    /// SkillRangeType: 0,1
    /// </summary>
    Near = 0,

    /// <summary>
    /// 最远
    /// SkillRangeType: 0,1
    /// </summary>
    Far = 1,

    /// <summary>
    /// 某项属性最小
    /// SkillRangeType: 0,1
    /// </summary>
    MinAttribute = 2,

    /// <summary>
    /// 某项属性最大
    /// SkillRangeType: 0,1
    /// </summary>
    MaxAttribute = 3,

    /// <summary>
    /// 最多目标
    /// SkillRangeType: 0,1
    /// 2015.12.16 23:48-log:这种类型空格子字段（IncludeEmpty）必须为false
    /// </summary>
    Max = 4,

    /// <summary>
    /// 对面
    /// SkillRangeType: 1
    /// 2015.12.16 23:48-log:对面打完，打旁边列，先左后右
    /// </summary>
    Opposite = 5,

    /// <summary>
    /// 随机
    /// </summary>
    Random = 6,

    /// <summary>
    /// 根据某项字段筛选
    /// 例如：国籍，职业等
    /// </summary>
    Field = 7,
}
