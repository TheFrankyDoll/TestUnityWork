using AxGrid.Base;
using UnityEngine;

public class CardsView : MonoBehaviourExt
{
    public Transform CardsParent;
    public GameObject CardPrefab;
    [Space(5)]
    public UICardGroup DrawnGroup;
    public UICardGroup TableGroup;


    Vector3 spawnPos = new Vector3(0, -500f);

    [OnStart]
    private void Subscribe()
    {
        Model.EventManager.AddAction<CardSO>(StateMachineT2.CardDrawnEvent, SpawnCard);
        Model.EventManager.AddAction<CardObject>(StateMachineT2.CardMovedEvent, MoveCard);
    }
    [OnDestroy]
    private void Unsubscribe()
    {
        Model.EventManager.RemoveAction<CardSO>(StateMachineT2.CardDrawnEvent, SpawnCard);
        Model.EventManager.RemoveAction<CardObject>(StateMachineT2.CardMovedEvent, MoveCard);
    }

    public void SpawnCard(CardSO data)
    {
        var obj = Instantiate(CardPrefab.gameObject, CardsParent).GetComponent<CardObject>();
        obj.Init(data);

        obj.rectTr.anchoredPosition = spawnPos;

        DrawnGroup.LinkedCardObjects.Add(obj);
        DrawnGroup.RedrawCardPlaces();
    }

    public void MoveCard(CardObject card)
    {
        DrawnGroup.LinkedCardObjects.Remove(card);
        DrawnGroup.RedrawCardPlaces();

        TableGroup.LinkedCardObjects.Add(card);
        TableGroup.RedrawCardPlaces();
    }
}
