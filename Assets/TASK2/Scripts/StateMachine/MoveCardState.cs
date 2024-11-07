using UnityEngine;
using AxGrid.FSM;
using AxGrid.Model;
using AxGrid;

public class MoveCardState : FSMState
{
    [Enter]
    void Enter()
    {
        var card = Model.Get<CardSO>(StateMachineT2.CardToMoveField);

        if (StateMachineT2.DrawnCards.Contains(card))
        {
            StateMachineT2.DrawnCards.Remove(card);
            Model.Refresh(StateMachineT2.DrawnCardsListName);

            StateMachineT2.TableCards.Add(card);
            Model.Refresh(StateMachineT2.TableCardsListName);

        }
        else if (StateMachineT2.TableCards.Contains(card))
        {
            StateMachineT2.TableCards.Remove(card);
            Model.Refresh(StateMachineT2.TableCardsListName);

            StateMachineT2.ThirdCards.Add(card);
            Model.Refresh(StateMachineT2.ThirdCardsListName);
        }
        else throw new System.IndexOutOfRangeException("Карта не находится в доступном для перемещения списке!");
        
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
