using AxGrid.FSM;
using AxGrid.Model;

public class AwaitState : FSMState
{
    readonly string nextState;
    public AwaitState(string nextState) => this.nextState = nextState;

    [Enter]
    void Enter()
    {
        Model.Set(StateMachineT3.properties.StartableField, true);
        Model.Set(StateMachineT3.properties.StopableField, false);
    }

    [Bind("OnBtn")]
    void onButtonClick(string buttonName)
    {
        if (buttonName == StateMachineT3.properties.StartButtonName)
        {
            Parent.Change(nextState);
        }
    }
}
