public class ActiveSkillData : SkillDataBase
{
    public ActiveSkillData()
    {
        this.SkillType = SkillType.Active;
    }
    public SelectionTargetType SelectionTarget;//选择目标的方式
    public AttributeType? SelectionTargetRefrenceAttribute;//选择目标参考属性
    public SkillRangeType SkillRange;//技能范围

    /*
     1.当 SkillRange 为 Cross 时
     eg:1,1,2,2 (上下左右)

     2.当 SkillRange 为 Rect 时
     eg:2,3

     3.当 SkillRange 为 Point 时

    */
    public byte[] RangeValue;

    public byte CD;//CD回合数
    //消耗率 消耗值 消耗的东西根据英雄的职业而定
    public RateOrValueType CostRateOrValue;
    public float CostValue;//数据
    public int[] TriggerBuffs;//触发的Buff
    public DamageType DamageType;//伤害类型
    public float Fluctuation;//波动
    public float? DamageRate;//造成的伤害率
    /// <summary>
    /// 回复生命 = 源（RestoreHPeSource） X 率（DamageRate）
    /// false(0)：表示施放方的攻击
    /// true(1)：表示作用放的血量
    /// </summary>
    public bool? RestoreHPeSource;

    /// <summary>
    /// 是否包含空格子
    /// </summary>
    public bool IncludeEmpty;

    /// <summary>
    /// 选取的数量
    /// </summary>
    public byte SelectionCount;

    /// <summary>
    /// 播放动画的次数
    /// </summary>
    public byte AnimationTimes;

    /// <summary>
    /// 移动到目标
    /// </summary>
    public MoveToTargetType MoveToTarget;

    /// <summary>
    /// 移动到目标的锚点
    /// </summary>
    public MoveToPivotType? MoveToPivot;



    /// <summary>
    /// 获取矩形范围的宽度
    /// </summary>
    /// <returns></returns>
    public byte GetRectWidth()
    {
        if (this.SkillRange == SkillRangeType.Rect)
        {
            return this.RangeValue[0];
        }
        return 0;
    }
    
    /// <summary>
     /// 获取矩形范围的高度
     /// </summary>
     /// <returns></returns>
    public byte GetRectHeight()
    {
        if (this.SkillRange == SkillRangeType.Rect)
        {
            return this.RangeValue[1];
        }
        return 0;
    }

    /// <summary>
    /// 十字 上
    /// </summary>
    /// <returns></returns>
    public byte GetCrossTop()
    {
        if (this.SkillRange == SkillRangeType.Cross)
        {
            return this.RangeValue[0];
        }
        return 0;
    }
    /// <summary>
    /// 十字 下
    /// </summary>
    /// <returns></returns>
    public byte GetCrossBottom()
    {
        if (this.SkillRange == SkillRangeType.Cross)
        {
            return this.RangeValue[1];
        }
        return 0;
    }
    /// <summary>
    /// 十字 左
    /// </summary>
    /// <returns></returns>
    public byte GetCrossLeft()
    {
        if (this.SkillRange == SkillRangeType.Cross)
        {
            return this.RangeValue[2];
        }
        return 0;
    }
    /// <summary>
    /// 十字 右
    /// </summary>
    /// <returns></returns>
    public byte GetCrossRight()
    {
        if (this.SkillRange == SkillRangeType.Cross)
        {
            return this.RangeValue[0];
        }
        return 0;
    }

}
