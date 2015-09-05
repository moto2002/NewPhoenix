/// <summary>
/// 战斗行为
/// </summary>
public enum FightActionType : byte
{
    /// <summary>
    /// 普通攻击
    /// </summary>
    NormalAttack,

    /// <summary>
    /// 绝技
    /// </summary>
    UniqueSkill,

    /// <summary>
    /// 主动技能
    /// </summary>
    ActiveSkill,

    /// <summary>
    /// 格挡
    /// </summary>
    Block,

    /// <summary>
    /// 闪避
    /// </summary>
    Dodge,

    /// <summary>
    /// 暴击
    /// </summary>
    Critical,

    /// <summary>
    /// 命中
    /// </summary>
    Hit,

    /// <summary>
    /// 破挡
    /// </summary>
    Broken,

    /// <summary>
    /// 抗暴
    /// </summary>
    OpposeCritical,

    /// <summary>
    /// 死亡
    /// </summary>
    Dead,

    /// <summary>
    /// 击杀
    /// </summary>
    Kill,

    /// <summary>
    /// 治疗
    /// </summary>
    Heal,

    /// <summary>
    /// 天气
    /// </summary>
    Weather,

}
