using AxGrid.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UICardGroup : MonoBehaviourExt
{
    private RectTransform rectTr;

    [Space(10)]

    public float CardWidth;
    public float PreferredSpacing = 5f;

    public float SidePadding = 5f;

    public float CardYChange = 2.5f;


    public List<CardObject> LinkedCardObjects = new List<CardObject>();

    [OnAwake]
    void Init()
    {
        rectTr = GetComponent<RectTransform>();
    }

    public void RedrawCardPlaces()
    {
        if (LinkedCardObjects == null || LinkedCardObjects.Count == 0) return;

        float cardInterval = CardWidth + PreferredSpacing;
        if(cardInterval * LinkedCardObjects.Count > rectTr.rect.width - SidePadding) cardInterval = (rectTr.rect.width - SidePadding) / LinkedCardObjects.Count;

        float cardX, cardY;

        cardX = rectTr.rect.center.x - (cardInterval * (float)(LinkedCardObjects.Count-1) * 0.5f);
        cardY = CardYChange;
        for (int i = 0; i < LinkedCardObjects.Count; i++)
        {
            LinkedCardObjects[i].MoveToNewPos(rectTr.TransformPoint(rectTr.rect.center + new Vector2(cardX, cardY)));

            cardX += cardInterval;
            cardY = i % 2 == 0 ? -CardYChange : CardYChange;
        }
    }
}
