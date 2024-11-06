using UnityEngine;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid;
using System.Collections.Generic;

public class StateMachineT2 : MonoBehaviourExt
{
    #region StaticProperties

    public const string CardSourcesField = "CardSources";

    public const string DrawnCardsListName = "DrawnCardsField";
    public static List<CardSO> DrawnCards
    {
        get => Settings.Model.Get<List<CardSO>>(DrawnCardsListName);
        set => Settings.Model.Set(DrawnCardsListName, value);
    }

    public const string TableCardsListName = "TableCardsField";
    public static List<CardSO> TableCards
    {
        get => Settings.Model.Get<List<CardSO>>(TableCardsListName);
        set => Settings.Model.Set(TableCardsListName, value);
    }

    public const string DrawButtonName = "DrawButton";

    public const string CanDrawField = "CanDrawCard";
    public const string CanMoveField = "CanMoveCard";

    public const string CardDrawnEvent = "OnCardDrawn";
    public const string CardMovedEvent = "OnCardMoved";

    public const string CardToMoveField = "CardToMove";
    public const string FSM_ReceiveCardToMove = "ReceiveCardToMove";

    public const int MaxDrawCards = 12;
    public const int MaxTableCards = 12;

    #endregion

    /// <summary> Проверит, можно ли забирать/двигать карты опираясь на их кол-во на столе. </summary>
    public static void CheckCardCounts()
    {
        Settings.Model.Set(CanDrawField, DrawnCards.Count < MaxDrawCards);
        Settings.Model.Set(CanMoveField, TableCards.Count < MaxTableCards);
    }

    [OnStart]
    private void Init()
    {
        var fsm = new FSM();
        Settings.Fsm = fsm;
        fsm.Add(new CardsInitState(), "CardsInit");
        fsm.Add(new DrawCardState(), "DrawCard");
        fsm.Add(new MoveCardState(), "MoveCard");

        fsm.Start("CardsInit");
    }

    [OnUpdate] void updateFsm() => Settings.Fsm.Update(Time.deltaTime);
}
