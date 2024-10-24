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
        Model.Set(StateMachine.properties.StartableField, false);

        Invoke(StateMachine.properties.SpinStartedEvent, speedupTime);
    }

    [One(speedupTime)]
    void toNextState()
    {
        Parent.Change(nextState);
    }
}
