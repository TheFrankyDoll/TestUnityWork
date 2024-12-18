﻿using UnityEngine;
using AxGrid.FSM;
using AxGrid.Model;
using AxGrid;

public class DrawCardState : FSMState
{
    [Enter]
    void Enter()
    {
        var cards = Model.Get<CardSO[]>(StateMachineT2.CardSourcesField);
        CardSO newCard = Object.Instantiate(cards[Random.Range(0, cards.Length)]);
        Invoke(StateMachineT2.CardDrawnEvent, newCard);

        StateMachineT2.DrawnCards.Add(newCard);
        Model.Refresh(StateMachineT2.DrawnCardsListName);

        StateMachineT2.CheckCardCounts();
    }

    [Bind("OnBtn")]
    void onDrawButtonClick(string buttonName)
    {
        if (buttonName == StateMachineT2.DrawButtonName)
        {
            Parent.Change("DrawCard");
        }
    }

    [Bind(StateMachineT2.FSM_ReceiveCardToMove)]
    void onCardClick(CardSO card)
    {
        if (StateMachineT2.DrawnCards.Contains(card))
        {
            if (!Model.GetBool(StateMachineT2.CanMoveField))
            {
                Log.Warn("Карта не может быть перемещена, так как на столе нет места.");
                return;
            }
        }
        else if (StateMachineT2.TableCards.Contains(card))
        {
            if (!Model.GetBool(StateMachineT2.CanThirdField))
            {
                Log.Warn("Карта не может быть перемещена, так как на 3-ем столе нет места.");
                return;
            }
        }
        else if (StateMachineT2.ThirdCards.Contains(card))
        {
            Log.Warn("Эта карта уже перемещена на последний стол");
            return;
        }
        else throw new System.IndexOutOfRangeException("Карта не находится в доступном для перемещения списке!");

        Parent.Change("MoveCard");
    }
}
