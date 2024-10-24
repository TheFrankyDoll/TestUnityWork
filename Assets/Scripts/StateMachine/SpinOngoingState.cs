using AxGrid.FSM;
using AxGrid.Model;

public class SpinOngoingState : FSMState
{
    readonly string nextState;
    public SpinOngoingState(string nextState) => this.nextState = nextState;

    [Enter]
    void Enter()
    {
        Model.Set(StateMachine.properties.StopableField, true);
    }

    [Bind("OnBtn")]
    void onButtonClick(string buttonName)
    {
        if (buttonName == StateMachine.properties.StopButtonName)
        {
            Parent.Change(nextState);
        }
    }
}
