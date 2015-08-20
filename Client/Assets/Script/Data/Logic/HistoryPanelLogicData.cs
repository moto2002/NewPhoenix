using System.Collections.Generic;

public class HistoryPanelLogicData
{
    public UIPanelType Panel { get; private set; }
    public PanelParamBase PanelParam { get; private set; }
    public HistoryPanelLogicData SecondPanel { get; private set; }

    public HistoryPanelLogicData(UIPanelType panel)
    {
        this.Panel = panel;
    }

    public HistoryPanelLogicData(UIPanelType panel, PanelParamBase param)
    {
        this.Panel = panel;
        this.SetPanelParam(param);
    }

    public HistoryPanelLogicData(UIPanelType panel, PanelParamBase param, HistoryPanelLogicData secondPanel)
    {
        this.Panel = panel;
        this.SetPanelParam(param);
        this.SetSecondPanel(secondPanel);
    }

    public void SetPanelParam(PanelParamBase param)
    {
        this.PanelParam = param;
    }

    public void SetSecondPanel(HistoryPanelLogicData data)
    {
        this.SecondPanel = data;
    }
}
