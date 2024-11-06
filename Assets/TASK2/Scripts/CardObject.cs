using AxGrid;
using AxGrid.Base;
using AxGrid.Path;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CardObject : MonoBehaviourExt, IPointerClickHandler, IPointerEnterHandler
{
    public RectTransform rectTr { get; private set; }

    public CardSO Data;
    [Space(5)]
    public Image ItemIcon;
    public TextMeshProUGUI ItemName;

    CPath movePath;    

    public void Init(CardSO data)
    {
        rectTr = GetComponent<RectTransform>();
        Data = data;

        Redraw();
    }

    public void Redraw()
    {
        ItemIcon.sprite = Data.itemSprite;
        ItemName.text = Data.cardName;
    }

    public void MoveToNewPos(Vector2 newPos, float AnimationTime = 0.28f)
    {
        Vector2 startPos = rectTr.position;

        movePath?.StopPath();

        movePath = CreateNewPath().EasingCircEaseInOut(AnimationTime, 0f, 1f, (delta) => { 
            rectTr.position = Vector2.Lerp(startPos, newPos, delta);
        });
    }


    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Model.Set(StateMachineT2.CardToMoveField, this);
        Settings.Fsm.Invoke(StateMachineT2.FSM_ReceiveCardToMove, Data);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => transform.SetAsLastSibling();
}
