using UnityEngine;

/// <summary>
/// X轴 表示长
/// z轴 表示宽
/// </summary>
public class FightGridComponent : ComponentBase
{
    /// <summary>
    /// 单元格子长
    /// </summary>
    public float GridXLength;

    /// <summary>
    /// 单元格子宽
    /// </summary>
    public float GridZLength;

    /// <summary>
    /// 长 格子数量
    /// </summary>
    public byte XGridCount;

    /// <summary>
    /// 敌方 宽 格子数量
    /// </summary>
    public byte EnemyZGridCount;

    /// <summary>
    /// 己方 宽 格子数量
    /// </summary>
    public byte PlayerZGridCount;

    /// <summary>
    /// 间隔 宽 
    /// </summary>
    public float BorderZLength;

    /// <summary>
    /// 暂时设定两个阵营一样宽，所以用一个变量来表示
    /// </summary>
    public byte ZGridCount
    {
        get { return this.PlayerZGridCount; }
    }
    public GridData[,] EnemyGrids { get; private set; }
    public GridData[,] PlayerGrids { get; private set; }
    public bool DrawGrid;

    [SerializeField]
    private float m_CenterX;
    [SerializeField]
    private float m_CenterZ;

    #region MonoBehaviour methods

    void OnDrawGizmos()
    {
        if (this.DrawGrid)
        {
            this.DrawEnemyGrid();
            this.DrawPlayerGrid();
            this.DrawBorder();
        }
    }

    #endregion

    #region override methods

    protected override void Awake()
    {
        base.Awake();
        this.InitEnemyGrid();
        this.InitPlayerGrid();
        this.InitCenter();
    }

    #endregion

    #region private methods

    private void InitEnemyGrid()
    {
        this.EnemyGrids = new GridData[this.EnemyZGridCount, this.XGridCount];
        for (byte z = 0; z < this.EnemyZGridCount; z++)
        {
            for (byte x = 0; x < this.XGridCount; x++)
            {
                this.EnemyGrids[z, x] = new GridData() { ZGrid = z, XGrid = x };
            }
        }
    }

    private void InitPlayerGrid()
    {
        this.PlayerGrids = new GridData[this.PlayerZGridCount, this.XGridCount];
        for (byte z = 0; z < this.PlayerZGridCount; z++)
        {
            for (byte x = 0; x < this.XGridCount; x++)
            {
                this.PlayerGrids[z, x] = new GridData() { ZGrid = z, XGrid = x };
            }
        }
    }

    private void DrawEnemyGrid()
    {
        for (byte z = 0; z < this.EnemyZGridCount; z++)
        {
            float positionZ = (z + this.m_CenterZ) * this.GridZLength;
            for (byte x = 0; x < this.XGridCount; x++)
            {
                float positionX = (x - this.m_CenterX) * this.GridXLength;
                Vector3 center = new Vector3(positionX, 0, positionZ);
                Vector3 size = new Vector3(this.GridXLength, 0, this.GridZLength);
                Gizmos.color = Color.red;
                Gizmos.DrawCube(center, size);
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }

    private void DrawPlayerGrid()
    {
        for (byte z = 0; z < this.PlayerZGridCount; z++)
        {
            float positionZ = -(z + this.m_CenterZ) * this.GridZLength;
            for (byte x = 0; x < this.XGridCount; x++)
            {
                float positionX = (x - this.m_CenterX) * this.GridXLength;
                Vector3 center = new Vector3(positionX, 0, positionZ);
                Vector3 size = new Vector3(this.GridXLength, 0, this.GridZLength);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(center, size);
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }

    private void DrawBorder()
    {
        Vector3 center = Vector3.zero;
        Vector3 size = new Vector3(this.GridXLength * this.XGridCount, 0, this.GridZLength);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(center, size);
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(center, size);
    }

    [ContextMenu("InitCenter")]
    private void InitCenter()
    {
        this.m_CenterX = this.XGridCount / 2.0f - 0.5f;
        this.m_CenterZ = (this.BorderZLength / 2) / this.GridZLength + 0.5f;
    }

    #endregion

    #region public methods

    public Vector3 ConvertGridToPosition(GridData data, ActorType type)
    {
        float positionX = (data.XGrid - this.m_CenterX) * this.GridXLength; ;
        float positionZ = (data.ZGrid + this.m_CenterZ) * this.GridZLength * ((type == ActorType.Enemy) ? 1 : -1);
        return new Vector3(positionX, 0, positionZ);
    }

    public GridData ConvertPositionToGrid(Vector3 position, ActorType type)
    {
        return new GridData();
    }

    public GridData ConvertIndexToGridData(byte index)
    {
        return new GridData()
        {
            ZGrid = (byte)(index / this.XGridCount),
            XGrid = (byte)(index % this.XGridCount)
        };
    }

    public byte ConvertGridDataToIndex(GridData data)
    {
        return (byte)(data.XGrid * this.XGridCount + data.ZGrid);
    }

    public byte GetMaxMagnitudeDistance()
    {
        return (byte)(Mathf.Pow(this.XGridCount, 2) + Mathf.Pow(this.PlayerZGridCount + this.EnemyZGridCount, 2));
    }

    public byte GetMaxIndex()
    {
        return (byte)(Mathf.Max(this.EnemyZGridCount, this.PlayerZGridCount) * this.XGridCount - 1);
    }

    /// <summary>
    /// 计算两个格子之间的距离的平方和
    /// </summary>
    /// <param name="grid1"></param>
    /// <param name="grid2"></param>
    /// <param name="isOtherCamp"表示是否为两个阵营的格子></param>
    /// <returns></returns>
    public byte CalculateMagnitudeDistance(GridData grid1, GridData grid2, bool isOtherCamp)
    {
        if (isOtherCamp)
        {
            return (byte)(Mathf.Pow(grid1.XGrid - grid2.XGrid, 2) + Mathf.Pow(grid1.ZGrid + grid2.ZGrid + 1, 2));
        }
        else
        {
            return (byte)(Mathf.Pow(grid1.XGrid - grid2.XGrid, 2) + Mathf.Pow(grid1.ZGrid - grid2.ZGrid, 2));
        }
    }

    public Vector3 GetMovesOffset(ActorType targetType)
    {
        switch (targetType)
        {
            case ActorType.Enemy:return new Vector3(0,0,-this.GridZLength/2);
            case ActorType.Player: return new Vector3(0, 0, this.GridZLength / 2);
        }
        return Vector3.zero;
    }

    public Vector3 GetMovesPosition(ActorBevBase targetBev)
    {
        return targetBev.MyTransform.position + this.GetMovesOffset(targetBev.Type);
    }

    #endregion
}
