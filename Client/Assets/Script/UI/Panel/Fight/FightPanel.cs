using System;

public class FightPanel : PanelBase
{
    public FightUIGridComponent PlayerGrid;
    public FightUIGridComponent EnemyGrid;


    protected override void Open(PanelParamBase panelParam)
    {
        this.PlayerGrid.InitGrid(FightMgr.Instance.PlayerGrids);
        this.EnemyGrid.InitGrid(FightMgr.Instance.EnemyGrids);

        this.PlayerGrid.UpdateBattleArray(FightMgr.Instance.PlayerBattleArray);
        this.EnemyGrid.UpdateBattleArray(FightMgr.Instance.EnemyBattleArray);
    }
}
