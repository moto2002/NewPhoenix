using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    public Camera UICam;
    public Camera MainCamera { get { return this.UICam; } }
    public Transform TopPanel;

    private Dictionary<UIPanelType,PanelBase> m_OpenedPanelDic = new Dictionary<UIPanelType, PanelBase>();
    private bool m_BackupUICameraStatus;
    private bool m_Dispose;
    private Stack<HistoryPanelLogicData> m_HistoryPanelStack = new Stack<HistoryPanelLogicData>();
    private bool m_HasPanelClosing;

    #region MonoBehaviour methods

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        this.m_Dispose = true;
    }

    #endregion

    #region private methods
 

    private int GetTopDepth()
    {
        int topDepth = int.MinValue;
        foreach (KeyValuePair<UIPanelType,PanelBase> kv in this.m_OpenedPanelDic)
        {
            int depth = kv.Value.GetTopDepth();
            if (depth > topDepth)
            {
                topDepth = depth;
            }
        }
        return topDepth;
    }

    #endregion

    public PanelBase GetOpenedPanelByType(UIPanelType type)
    {
        return (this.m_OpenedPanelDic.ContainsKey(type))? this.m_OpenedPanelDic[type] : null;
    }

    #region OpenPanel

    public void OpenPanel(UIPanelType type, PanelParamBase panelParam = null,PanelEffectType openEffectType = PanelEffectType.Open)
    {
        PanelBase panel = null;
        int newDepth = 0;
        if (this.m_OpenedPanelDic.ContainsKey(type))
        {
            panel = this.m_OpenedPanelDic[type];
            newDepth = panel.GetDepth();
        }
        else
        {
            panel = PoolMgr.Instance.GetPanel(type);
            if (panel == null)
            {
                Debug.Log("Panel is null " + type);
                return;
            }
            panel.MyTransform.localPosition = Vector3.zero;
            this.m_OpenedPanelDic.Add(type, panel);
            newDepth = this.GetTopDepth() + 2;//加2的原因是为了保险起见，因为有的时候只加1可能会出现面板重叠
        }
        panel.Open(type,panelParam, openEffectType, newDepth);
    }

    #endregion

    #region ClosePanel

    public void ClosePanel(UIPanelType type,PanelEffectType closeEffectType = PanelEffectType.Close)
    {
        if (this.m_OpenedPanelDic.ContainsKey(type))
        {
            this.m_OpenedPanelDic[type].Close(closeEffectType);
            this.m_OpenedPanelDic.Remove(type);
        }
        else
        {
            Debug.LogError(type + " is null");
        }
    }

    public void ClosePanel(HistoryPanelLogicData data)
    {
        if (data != null)
        {
            this.ClosePanel(data.Panel,PanelEffectType.CloseByOpenOther);
            this.ClosePanel(data.SecondPanel);
        }
    }


    public void TryClosePanel(UIPanelType type)
    {
        if (this.m_OpenedPanelDic.ContainsKey(type))
        {
            this.ClosePanel(type);
        }
        else
        {
            //Debug.Log("这个提示可以不用管 Close panel failed  " + type + " is not exist");
        }
    }

    public void CloseAllOpendPanel()
    {
        if (this.m_OpenedPanelDic != null && this.m_OpenedPanelDic.Count > 0)
        {
            foreach (KeyValuePair<UIPanelType, PanelBase> kv in this.m_OpenedPanelDic)
            {
                kv.Value.Dispose();
            }
            this.m_OpenedPanelDic.Clear();
        }
    }


    #endregion

    #region NotifyPanel 

    public void NotifyPanel(UIPanelType type, string methodName, object param = null)
    {
        PanelBase panel = this.GetOpenedPanelByType(type);
        if (panel != null)
        {
            panel.SendMessage(methodName, param,SendMessageOptions.DontRequireReceiver);
        }
    }
    #endregion

    #region History

    public void RecordHistory(UIPanelType panel)
    {
        this.RecordHistory(new HistoryPanelLogicData(panel) );
    }

    public void RecordHistory(HistoryPanelLogicData history)
    {
        this.m_HistoryPanelStack.Push(history);
    }

    public void BackToHistory()
    {
        if (this.m_HistoryPanelStack.Count > 0)
        {
            this.OpenHistory(this.m_HistoryPanelStack.Pop());
        }
    }

    private void OpenHistory(HistoryPanelLogicData history)
    {
        this.OpenPanel(history.Panel,history.PanelParam,PanelEffectType.OpenByCloseOther);
        if (history.SecondPanel != null)
        {
            this.OpenHistory(history.SecondPanel);
        }
    }

    public void BackToHistory(PanelParamBase newPanelParam)
    {
        if (this.m_HistoryPanelStack.Count > 0)
        {
            this.OpenHistory(this.m_HistoryPanelStack.Pop(), newPanelParam);
        }
    }

    private void OpenHistory(HistoryPanelLogicData history, PanelParamBase newPanelParam)
    {
        this.OpenPanel(history.Panel, newPanelParam);
        if (history.SecondPanel != null)
        {
            this.OpenHistory(history.SecondPanel);
        }
    }

    public void ClearHistory()
    {
        this.m_HistoryPanelStack.Clear();
    }

    public void ChangeHistoryPanelParam(PanelParamBase param)
    {
        if (this.m_HistoryPanelStack.Count > 0)
        {
            HistoryPanelLogicData data = this.m_HistoryPanelStack.Pop();
            data.SetPanelParam(param);
            this.m_HistoryPanelStack.Push(data);
        }
    }

    public void ChangeHistoryPanelParam(HistoryPanelLogicData secondPanel)
    {
        if (this.m_HistoryPanelStack.Count > 0)
        {
            HistoryPanelLogicData data = this.m_HistoryPanelStack.Pop();
            data.SetSecondPanel(secondPanel);
            this.m_HistoryPanelStack.Push(data);
        }
    }

    #endregion
}

