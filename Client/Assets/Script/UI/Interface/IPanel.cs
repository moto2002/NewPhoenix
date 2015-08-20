using System;

public interface IPanel
{
    void Open(UIPanelType type, PanelParamBase panelParam, PanelEffectType effectType, int depth);
    void Close(PanelEffectType type);
}
