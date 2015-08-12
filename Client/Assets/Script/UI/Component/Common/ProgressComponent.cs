using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ProgressComponent : ComponentBase
{
    #region public members

    public Slider Slider;
    public Text FractionText;

    [Range(1, 1000)]
    public int ProgressSpeed = 1;// 速度
    public Vector2 Size;
    public event Action IteratorEvent;

    /// <summary>
    /// 值，0.0f-1.0f
    /// </summary>
    public float Value { get { return this.Slider.value; } }

    /// <summary>
    /// 是否暂停
    /// </summary>
    public bool Pause { get; set; }

    private float m_DeltaNumber = 0.0001f;
    private bool m_Progressing;
    private float m_EndValue;
    private float m_Offset;
    private int m_IterationTimes;


    void LateUpdate()
    {
        if (this.m_Progressing && !this.Pause)
        {
            float increment = this.m_DeltaNumber * this.ProgressSpeed;
            if (this.m_Offset > 0)
            {
                this.m_Offset -= increment;
                this.UpdateProgressValue(this.Slider.value + increment);
                if (this.m_Offset <= 0)
                {
                    this.UpdateProgressValue(this.Slider.value + this.m_Offset);
                    this.UpdateProgressValue(this.m_EndValue);
                    this.StopProgress();
                }
            }
            else if (this.m_Offset < 0)
            {
                this.m_Offset += increment;
                this.UpdateProgressValue(this.Slider.value - increment);
                if (this.m_Offset >= 0)
                {
                    this.UpdateProgressValue(this.Slider.value - m_Offset);
                    this.UpdateProgressValue(this.m_EndValue);
                    this.StopProgress();
                }
            }
        }
    }

    #endregion

    #region override methods

    public override void Clear()
    {
        this.StopProgress();
        this.UpdateProgressValue(0);
        if (this.FractionText != null)
        {
            this.FractionText.text = string.Empty;
        }
    }

    #endregion

    #region private methods

    /// <summary>
    /// 产生动画效果
    /// value：最终值
    /// iteration：迭代次数
    /// </summary>
    /// <param name="value"></param>
    /// <param name="iteration"></param>
    /// <returns></returns>
    private IEnumerator ProgressIteratorForIteration(float value, int iteration, float delayTime)
    {
        //Debug.Log("ProgressInspectorForIteration : " + delayTime);
        yield return new WaitForSeconds(delayTime);
        //Debug.Log("this.IsOpen: " + this.IsOpen);

        if (!this.IsOpen)
        {
            yield break;
        }
        float increment = this.m_DeltaNumber * ProgressSpeed;
        for (int i = 0; i <= iteration; i++)
        {
            float endValue = 1;
            if (i == iteration)
            {
                endValue = value;
            }
            //Debug.Log("tmpValue : " + endValue);
            while (this.Slider.value < endValue)
            {
                if (!this.Pause)
                {
                    this.UpdateProgressValue(this.Slider.value + increment);
                    //Debug.Log("1111111111endValue" + endValue + "  111111this.Progress.value : " + this.Progress.value);
                }
                yield return 1;
            }
            if (i < iteration)
            {
                this.UpdateProgressValue(0);
                if (this.IteratorEvent != null)
                {
                    this.IteratorEvent();
                }
            }
        }
        //Debug.Log("value : " + value + "  this.Progress.value : " + this.Progress.value);
    }

    [ContextMenu("Set Size")]
    private void SetSize()
    {
        this.Size.x = (int)(Mathf.Max(0, Size.x));
        this.Size.y = (int)(Mathf.Max(0, Size.y));
        Image[] images = this.GetComponentsInChildren<Image>(true);
        foreach (Image image in images)
        {
            //image.minWidth = (int)this.Size.x;
            //image.height = (int)this.Size.y;
        }
    }

  

    private void UpdatePercent()
    {
        //if (this.PercentLabel != null)
        //{
        //    if (this.Value == 0)
        //    {
        //        this.PercentLabel.text = string.Empty;
        //    }
        //    else
        //    {
        //        this.PercentLabel.text = (int)(this.Value * 100) + "%";
        //    }
        //}
    }

    private void UpdateProgressValue(float value)
    {
        //Debug.Log("UpdateProgressValue " + value);
        this.Slider.value = value;
        this.UpdateThumb();
        this.UpdatePercent();
    }

    private void UpdateThumb()
    {
        //if (this.Light != null)
        //{
        //    //Debug.Log("  UpdateThumb " + this.Progress.value +"<= 0 || this.Progress.value >= 1");
        //    if (this.Progress.value <= 0 || this.Progress.value >=1)
        //    {
        //        this.Light.gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        this.Light.gameObject.SetActive(true);
        //    }
        //}
    }

    #endregion

    #region public methods

    public void UpdateValue(int current, int total, bool immediate)
    {
        if (total > 0 && current >= 0)
        {
            float value = current / (float)total;
            this.UpdateValue(value, immediate);
            this.UpdateFraction(current, total);
        }
    }

    public void UpdateValue(float oldValue,float newValue, int iterationTimes, float delayTime)
    {
        this.UpdateProgressValue(oldValue);
        if (this.gameObject.activeInHierarchy)
        {
            //Debug.Log("this.gameObject.activeInHierarchy");
            StopAllCoroutines();
            StartCoroutine(this.ProgressIteratorForIteration(newValue, iterationTimes, delayTime));
        }
    }

    public void UpdateFraction(int current, int total)
    {
        if (this.FractionText != null)
        {
            this.FractionText.text = current + "/" + total;
        }
    }

    public void UpdateValue(float value, bool isImmediate)
    {
        //Debug.Log("UpdateValue " + value+ "  isImmediate " + isImmediate);
        float newValue = Mathf.Clamp01(value);
        this.m_Offset = newValue - this.Slider.value;
        if (Mathf.Abs(this.m_Offset) <= float.Epsilon)
        {
            this.StopProgress();
            this.UpdateProgressValue(newValue);
        }
        else
        {
            if (isImmediate)
            {
                this.StopProgress();
                this.UpdateProgressValue(newValue);
            }
            else
            {
                this.m_EndValue = newValue;
                this.m_Progressing = true;
            }
        }
    }

    public void StopProgress()
    {
        this.m_Progressing = false;
    }

    public bool IsFull()
    {
        return this.Value >= 1;
    }

    public void UpdateLabel(string str)
    {
        if (this.FractionText != null)
        {
            this.FractionText.text = str;
        }
    }

    /// <summary>
    /// 通过时间更新进度条
    /// </summary>
    /// <param name="value"></param>
    /// <param name="seconds"></param>
    public void UpdateValue(float value, long seconds)
    {
        //165.0f这个值是手动测出来的（2015.05.27）
        this.SetSpeed((int)( 165.0f /seconds));
        this.UpdateValue(value,false);
    }

    public void SetSpeed(int speed)
    {
        this.ProgressSpeed = speed;
    }

    #endregion
}