using UnityEngine;
using AxGrid.FSM;
using AxGrid.Model;
using System.Collections.Generic;

public class CardsInitState : FSMState
{
    [Enter]
    void Enter()
    {
        Model.Set(StateMachineT2.CardSourcesField, Resources.LoadAll<CardSO>("TASK2/CardSources"));

        StateMachineT2.DrawnCards = new List<CardSO>();
        StateMachineT2.TableCards = new List<CardSO>();

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
}
