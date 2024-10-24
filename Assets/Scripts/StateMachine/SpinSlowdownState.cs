using AxGrid.FSM;
using AxGrid.Model;

public class SpinSlowdownState : FSMState
{
    readonly string nextState;
    public SpinSlowdownState(string nextState) => this.nextState = nextState;

    const float slowdownTime = 0.8f;

    [Enter]
    void Enter()
    {
        Model.Set(StateMachine.properties.StopableField, false);

        Invoke(StateMachine.properties.SpinSlowdownEvent, slowdownTime);
    }

    [Exit]
    void Exit()
    {
        Invoke(StateMachine.properties.SpinStopEvent);
    }

    [One(slowdownTime)]
    private void toNextState()
    {
        Parent.Change(nextState);
    }
}
