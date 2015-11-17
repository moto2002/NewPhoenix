using System.Collections.Generic;

public class HistoryPanelLogicData
{
    public PanelType Panel { get; private set; }
    public PanelParamBase PanelParam { get; private set; }
    public HistoryPanelLogicData SecondPanel { get; private set; }

    public HistoryPanelLogicData(PanelType panel)
    {
        this.Panel = panel;
    }

    public HistoryPanelLogicData(PanelType panel, PanelParamBase param)
    {
        this.Panel = panel;
        this.SetPanelParam(param);
    }

    public HistoryPanelLogicData(PanelType panel, PanelParamBase param, HistoryPanelLogicData secondPanel)
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
