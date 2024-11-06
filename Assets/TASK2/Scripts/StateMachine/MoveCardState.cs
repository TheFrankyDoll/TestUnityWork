using UnityEngine;
using AxGrid.FSM;
using AxGrid.Model;
using AxGrid;

public class MoveCardState : FSMState
{
    [Enter]
    void Enter()
    {
        var card = Model.Get<CardObject>(StateMachineT2.CardToMoveField);

        StateMachineT2.DrawnCards.Remove(card.Data);
        StateMachineT2.TableCards.Add(card.Data);
        Invoke(StateMachineT2.CardMovedEvent, card);

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
        if (!Model.GetBool(StateMachineT2.CanMoveField)) {
            Log.Warn("Карта не может быть перемещена, так как на столе нет места.");
            return;
        }
        if (StateMachineT2.TableCards.Contains(card)) {
            Log.Warn("Эта карта уже перемещена на стол");
            return;
        }

        Parent.Change("MoveCard");
    }
}
