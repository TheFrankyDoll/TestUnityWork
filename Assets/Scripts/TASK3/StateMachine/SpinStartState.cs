using AxGrid.FSM;
using AxGrid.Model;

public class SpinStartState : FSMState
{
    readonly string nextState;
    public SpinStartState(string nextState) => this.nextState = nextState;

    const float speedupTime = 3f;

    [Enter]
    void Enter()
    {
        Model.Set(StateMachineT3.properties.StartableField, false);

        Invoke(StateMachineT3.properties.SpinStartedEvent, speedupTime);
    }

    [One(speedupTime)]
    void toNextState()
    {
        Parent.Change(nextState);
    }
}
