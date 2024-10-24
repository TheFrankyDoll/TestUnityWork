using AxGrid.Base;
using UnityEngine;
using UnityEngine.UI;

public class RouletteCell : MonoBehaviourExt
{
    public SlotItemSO item {  get; private set; }

    [Space(5)]
    [SerializeField] Image itemImage;

    RectTransform _rectTransform;
    public RectTransform RectTransform {
        get
        {
            if(!_rectTransform) _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    public virtual void Init(SlotItemSO item)
    {
        this.item = item;
        Redraw();
    }
    public virtual void SetPosition(Vector2 pos) => RectTransform.anchoredPosition = pos;

    public void Redraw()
    {
        itemImage.sprite = item.ItemSprite;
    }
}
