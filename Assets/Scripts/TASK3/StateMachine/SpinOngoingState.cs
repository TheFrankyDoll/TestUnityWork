using AxGrid.FSM;
using AxGrid.Model;

public class SpinOngoingState : FSMState
{
    readonly string nextState;
    public SpinOngoingState(string nextState) => this.nextState = nextState;

    [Enter]
    void Enter()
    {
        Model.Set(StateMachineT3.properties.StopableField, true);
    }

    [Bind("OnBtn")]
    void onButtonClick(string buttonName)
    {
        if (buttonName == StateMachineT3.properties.StopButtonName)
        {
            Parent.Change(nextState);
        }
    }
}
