/// <summary>
/// 破军武将 护盾类型
/// </summary>
public enum ShieldType : byte
{
    /// <summary>
    /// 先使用护盾再使用生命
    /// </summary>
    BeforeHP = 0,

    /// <summary>
    /// 先使用生命再使用护盾
    /// </summary>
    AfterHP = 1,
}
