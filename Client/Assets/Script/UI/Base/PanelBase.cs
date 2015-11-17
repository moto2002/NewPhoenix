using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public abstract class PanelBase : UIBase, IPanel
{
    public PanelType ThisPanel { get; private set; }
    private UIEffectBase m_PanelEffect;

    void OnDestory()
    {
        StopAllCoroutines();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        this.m_PanelEffect = this.GetComponent<UIEffectBase>();
    }

    protected virtual void InitPanel() { }
    protected abstract void Open(PanelParamBase panelParam);
    public virtual void InClose() { this.CloseThisPanel(); }
    protected virtual void OpenComplete()
    {
        this.MyTransform.localScale = Vector3.one * 0.99f;
        this.MyTransform.localScale = Vector3.one;
    }

    protected virtual void CloseComplete()
    {
        this.Dispose();
    }

    public override void Clear()
    {
        base.Clear();
        StopAllCoroutines();
    }


    public virtual void Close(PanelEffectType type)
    {
        //在关闭面板时，禁用Collider,防止在播放动画的时候再次点到
        UIUtils.SetUIColliderEnable(this.MyTransform, false);
        this.PlayEffect(type);
    }

    #region private methods

    private void PlayEffect(PanelEffectType type)
    {
        if (this.m_PanelEffect == null)
        {
            if (type == PanelEffectType.Close || type == PanelEffectType.CloseByOpenOther)
            {
                this.CloseComplete();
            }
            else if (type == PanelEffectType.Open || type == PanelEffectType.OpenByCloseOther)
            {
                this.OpenComplete();
            }
        }
        else
        {
            this.m_PanelEffect.Play(type);
        }
    }

    #endregion

    #region protected methods

    /// <summary>
    /// 关闭当前面板
    /// </summary>
    protected void CloseThisPanel(PanelEffectType closeEffectType = PanelEffectType.Close)
    {
        if (this.ThisPanel == PanelType.None) { Debug.Log("this panel is none can't close!"); return; }
        UICtrller.Instance.ClosePanel(this.ThisPanel, closeEffectType);
    }

    #endregion

    #region public methods

    public void Open(PanelType type, PanelParamBase panelParam, PanelEffectType effectType, int depth)
    {
        this.ThisPanel = type;
        this.SetPanelDepth(depth);
        if (!this.IsInit) this.Init();
        this.InitPanel();
        this.Open(panelParam);
        this.PlayEffect(effectType);
    }

    public void SetPanelDepth(int depth)
    {
        if (this.GetComponent<FixedPanelDepthTool>() != null)
        {
            return;
        }
       

    }

    public int GetTopDepth()
    {
        if (this.GetComponent<FixedPanelDepthTool>() != null)
        {
            return 0;
        }
        int topDepth = 0;
        return topDepth;
    }

    public int GetDepth()
    {
        return 0;
    }

    #endregion
}
