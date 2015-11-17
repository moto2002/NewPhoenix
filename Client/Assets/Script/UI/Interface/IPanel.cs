using System;

public interface IPanel
{
    void Open(PanelType type, PanelParamBase panelParam, PanelEffectType effectType, int depth);
    void Close(PanelEffectType type);
}
