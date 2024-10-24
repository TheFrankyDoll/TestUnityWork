using AxGrid.Base;
using AxGrid.Path;
using UnityEngine;

public class SlotParticle : MonoBehaviourExt
{
    [SerializeField] ParticleSystem PS;

    SlotItemSO item;
    CPath speedPath;

    public void Init(SlotItemSO item)
    {
        this.item = item;
        PS.textureSheetAnimation.AddSprite(item.ItemSprite);
    }

    [OnEnable]
    private void Subscribe()
    {
        Model.EventManager.AddAction<float>(StateMachine.properties.SpinStartedEvent, StartFlow);
        Model.EventManager.AddAction<float>(StateMachine.properties.SpinSlowdownEvent, SlowdownFlow);
        Model.EventManager.AddAction<SlotItemSO>(StateMachine.properties.SpinFullStopEvent, EndFall);
    }

    [OnDisable]
    private void Unsubscribe()
    {
        Model.EventManager.RemoveAction<float>(StateMachine.properties.SpinStartedEvent, StartFlow);
        Model.EventManager.RemoveAction<float>(StateMachine.properties.SpinSlowdownEvent, SlowdownFlow);
        Model.EventManager.RemoveAction<SlotItemSO>(StateMachine.properties.SpinFullStopEvent, EndFall);
    }

    void StartFlow(float speedUpTime)
    {
        speedPath?.StopPath();
        
        PS.Play();
        var psMain = PS.main;
        psMain.gravityModifier = 0f;

        float startSpeed = psMain.simulationSpeed;
        speedPath = CreateNewPath()
            .EasingQuadEaseOut(speedUpTime, startSpeed, 1f, delta => {
                psMain.simulationSpeed = delta;
            });
    }
    void SlowdownFlow(float speedDownTime)
    {
        speedPath?.StopPath();

        var psMain = PS.main;
        float startSpeed = psMain.simulationSpeed;
        speedPath = CreateNewPath()
            .EasingCircEaseOut(speedDownTime, startSpeed, 0f, delta => {
                psMain.simulationSpeed = delta;
            }).Action(PS.Stop);
    }

    void EndFall(SlotItemSO reward)
    {
        speedPath?.StopPath();

        if (reward.ItemSprite == item.ItemSprite) return;
        var psMain = PS.main;
        psMain.simulationSpeed = 1f;
        psMain.gravityModifier = 3f;
        PS.Stop();
    }
}
