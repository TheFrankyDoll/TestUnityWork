using AxGrid.Base;
using AxGrid.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UICardGroup : MonoBehaviourExtBind
{
    private RectTransform rectTr;

    public string RelatedCardsList;

    [Space(10)]

    public float CardWidth;
    public float PreferredSpacing = 5f;

    public float SidePadding = 5f;

    public float CardYChange = 2.5f;

    [OnAwake]
    void Init()
    {
        rectTr = GetComponent<RectTransform>();
    }

    [Bind("On{RelatedCardsList}Changed")]
    public void RedrawCardPlaces()
    {
        var cardList = Model.Get<List<CardSO>>(RelatedCardsList);
        CardObject[] DrawnCards = new CardObject[cardList.Count];

        for (int i = 0; i < cardList.Count; i++)
        {
            DrawnCards[i] = CardsView.ObjectFromCard[cardList[i]];
        }

        if (DrawnCards.Length == 0) return;

        float cardInterval = CardWidth + PreferredSpacing;
        if(cardInterval * DrawnCards.Length > rectTr.rect.width - SidePadding) cardInterval = (rectTr.rect.width - SidePadding) / DrawnCards.Length;

        float cardX, cardY;

        cardX = rectTr.rect.center.x - (cardInterval * (float)(DrawnCards.Length -1) * 0.5f);
        cardY = CardYChange;
        for (int i = 0; i < DrawnCards.Length; i++)
        {
            DrawnCards[i].MoveToNewPos(rectTr.TransformPoint(rectTr.rect.center + new Vector2(cardX, cardY)));

            cardX += cardInterval;
            cardY = i % 2 == 0 ? -CardYChange : CardYChange;
        }
    }
}
