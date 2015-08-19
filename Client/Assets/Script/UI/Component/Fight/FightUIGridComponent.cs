using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FightUIGridComponent : ComponentBase
{
    public FightUIGridCellComponent CellPrefab;
    public ActorType Type;

    private Dictionary<FightUIGridCellComponent, GridData> m_GridDic;

    #region private methods



    #endregion

    #region public methods

    public void InitGrid(GridData[,] grids)
    {
        this.m_GridDic = new Dictionary<FightUIGridCellComponent, GridData>();
        int zLength = grids.GetLength(0);
        int xLength = grids.GetLength(1);
        float cellLength = this.CellPrefab.CellLength;
        for (int z = 0; z < zLength; z++)
        {
            float positionY =((this.Type == ActorType.Player)?(zLength - z): z) * cellLength;
            //float positionY  = z * cellLength;
            for (int x = 0; x < xLength; x++)
            {
                FightUIGridCellComponent cell = Instantiate(this.CellPrefab);
                cell.MyTransform.SetParent(this.MyTransform);
                cell.MyTransform.localRotation = Quaternion.identity;
                float positionX = x * cellLength;
                cell.MyTransform.localPosition = new Vector3(positionX, positionY, 0);
                this.m_GridDic.Add(cell, grids[z, x]);
            }
        }
    }

    /// <summary>
    /// 更新阵容
    /// </summary>
    /// <param name=""></param>
    public void UpdateBattleArray(Dictionary<ActorBevBase, GridData> battleArrayDic)
    {
        foreach (KeyValuePair<ActorBevBase, GridData> kv in battleArrayDic)
        {
            FightUIGridCellComponent cell = this.m_GridDic.First(a => a.Value.Equals(kv.Value)).Key;
            cell.SetActor(kv.Key);
        }
    }

    #endregion


}
