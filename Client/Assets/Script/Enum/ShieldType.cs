/// <summary>
/// 破军武将 护盾类型
/// </summary>
public enum ShieldType : byte
{
    /// <summary>
    /// 没有护盾属性
    /// </summary>
    None = 0,

    /// <summary>
    /// 先使用护盾再使用生命
    /// </summary>
    BeforeHP = 1,

    /// <summary>
    /// 先使用生命再使用护盾
    /// </summary>
    AfterHP = 2,
}
