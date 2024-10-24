using AxGrid.FSM;
using AxGrid.Model;

public class AwaitState : FSMState
{
    readonly string nextState;
    public AwaitState(string nextState) => this.nextState = nextState;

    [Enter]
    void Enter()
    {
        Model.Set(StateMachine.properties.StartableField, true);
        Model.Set(StateMachine.properties.StopableField, false);
    }

    [Bind("OnBtn")]
    void onButtonClick(string buttonName)
    {
        if (buttonName == StateMachine.properties.StartButtonName)
        {
            Parent.Change(nextState);
        }
    }
}
