using AxGrid.Base;
using System.Collections.Generic;
using UnityEngine;

public class CardsView : MonoBehaviourExt
{
    public static Dictionary<CardSO, CardObject> ObjectFromCard = new Dictionary<CardSO, CardObject>();

    public Transform CardsParent;
    public GameObject CardPrefab;

    Vector3 spawnPos = new Vector3(500f, -800f);

    [OnStart]
    private void Subscribe()
    {
        Model.EventManager.AddAction<CardSO>(StateMachineT2.CardDrawnEvent, SpawnCard);
    }
    [OnDestroy]
    private void Unsubscribe()
    {
        Model.EventManager.RemoveAction<CardSO>(StateMachineT2.CardDrawnEvent, SpawnCard);
    }

    public void SpawnCard(CardSO data)
    {
        var obj = Instantiate(CardPrefab.gameObject, CardsParent).GetComponent<CardObject>();
        obj.Init(data);

        obj.rectTr.anchoredPosition = spawnPos;

        ObjectFromCard.Add(data, obj);
    }
}
