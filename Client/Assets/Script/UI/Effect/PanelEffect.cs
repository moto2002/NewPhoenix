using System;
using System.Collections;
using UnityEngine;

public class PanelEffect : UIEffectBase
{
    private float m_EffectTime = 0.12f;

    void Awake()
    {
    }

    public override void Play(object o)
    {
        float delayTime = 0;
        float fromAlpha = 0;
        float toAlpha = 0;
        Vector3 fromScale = Vector3.zero;
        Vector3 toScale = Vector3.zero;
        string completeFunction = string.Empty;
        switch ((PanelEffectType)o)
        {
            case PanelEffectType.Close:
                fromAlpha = 1;
                toAlpha = 0;
                fromScale = Vector3.one;
                toScale = new Vector3(0.8f, 0.8f, 1);
                completeFunction = "CloseComplete";
                break;
            case PanelEffectType.CloseByOpenOther:
                fromAlpha = 1;
                toAlpha = 0;
                fromScale = Vector3.one;
                toScale = new Vector3(1.2f, 1.2f, 1);
                completeFunction = "CloseComplete";
                break;
            case PanelEffectType.Open:
                fromAlpha = 0;
                toAlpha = 1;
                fromScale = new Vector3(0.8f, 0.8f, 1);
                toScale = Vector3.one;
                completeFunction = "OpenComplete";
                delayTime = this.m_EffectTime;
                break;
            case PanelEffectType.OpenByCloseOther:
                fromAlpha = 0;
                toAlpha = 1;
                fromScale = new Vector3(1.2f, 1.2f, 1);
                toScale = Vector3.one;
                completeFunction = "OpenComplete";
                delayTime = this.m_EffectTime;
                break;
        }
        this.UpdateAlpha(fromAlpha);
        this.UpdateScale(fromScale);
        StartCoroutine(this.Effect(delayTime, toAlpha, toScale, completeFunction));
    }

    private IEnumerator Effect(float delayTime, float toAlpha, Vector3 toScale, string completeFunction)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < delayTime)
        {
            yield return 1;
        }
        startTime = Time.realtimeSinceStartup;
        float time = 0;
        while (time <= this.m_EffectTime)
        {
            float t = time / this.m_EffectTime;
            this.UpdateScale(Vector3.Slerp(this.MyTransform.localScale, toScale, t));
            yield return 1;
            time = Time.realtimeSinceStartup - startTime;
        }
        this.MyGameObject.SendMessage(completeFunction, null, SendMessageOptions.RequireReceiver);
    }

    private void UpdateAlpha(float alpha)
    {
    }

    private void UpdateScale(Vector3 scale)
    {
        this.MyTransform.localScale = scale;
    }

    public override void Play() { }
}
