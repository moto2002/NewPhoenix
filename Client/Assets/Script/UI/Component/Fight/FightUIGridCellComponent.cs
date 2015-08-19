using UnityEngine.UI;
using UnityEngine;

public class FightUIGridCellComponent : ComponentBase
{
    public Image CellImage;
    public Sprite InArraySprite;
    public GameObject ActorSprite;
    public float CellLength { get { return this.CellImage.rectTransform.sizeDelta.x; } }

    private Sprite m_DefaultSprite;
    private ActorBevBase m_ActorBev;

    protected override void Awake()
    {
        base.Awake();
        this.m_DefaultSprite = this.CellImage.sprite;
    }

    /// <summary>
    /// 设置状态
    /// true:在阵上
    /// false:不在阵上
    /// </summary>
    /// <param name="inArray"></param>
    public void SetStatus(bool inArray)
    {
        this.CellImage.sprite = inArray ? this.InArraySprite : this.m_DefaultSprite;
    }

    /// <summary>
    /// 是否有角色
    /// </summary>
    /// <param name="hasActor"></param>
    public void SetHasActor(bool hasActor)
    {
        this.ActorSprite.SetActive(hasActor);
    }

    public void SetActor(ActorBevBase actor)
    {
        this.m_ActorBev = actor;
        this.SetHasActor(true);
    }
}
