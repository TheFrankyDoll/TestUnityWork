using UnityEngine;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid;

public class StateMachineT3 : MonoBehaviourExt
{
    public readonly static Properties properties = new()
    {
        StartableField = "RouletteIsStartable",
        StopableField = "RouletteIsStopable",

        StartButtonName = "ButtonStart",
        StopButtonName = "ButtonStop",

        SpinStartedEvent = "SpinStarted",
        SpinSlowdownEvent = "SpinSlowdown",
        SpinStopEvent = "SpinStopping",
        SpinFullStopEvent = "SpinFullStop",
    };

    FSM fsm;

    [OnStart]
    private void Init()
    {
        fsm = new FSM();
        Settings.Fsm = fsm;
        fsm.Add(new AwaitState(nextState: "SpinStart"), "Await");
        fsm.Add(new SpinStartState(nextState: "SpinOngoing"), "SpinStart");
        fsm.Add(new SpinOngoingState(nextState: "SpinSlowdown"), "SpinOngoing");
        fsm.Add(new SpinSlowdownState(nextState: "Await"), "SpinSlowdown");

        fsm.Start("Await");
    }

    [OnUpdate] void updateFsm() => fsm.Update(Time.deltaTime);

    [System.Serializable]
    public struct Properties
    {
        public string StartableField;
        public string StopableField;

        public string StartButtonName;
        public string StopButtonName;

        public string SpinStartedEvent;
        public string SpinSlowdownEvent;
        public string SpinStopEvent;

        public string SpinFullStopEvent;
    }
}