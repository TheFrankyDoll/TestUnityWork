using AxGrid.Base;
using AxGrid.Path;
using UnityEngine;
using UnityEngine.UI;

public class SlotBG : MonoBehaviourExt
{
    [SerializeField] Image bg;
    [SerializeField] float ResetTime;
    [SerializeField] float RewardTime;
    [Space(5)]
    [SerializeField] Color NormalColor;

    CPath colorPath;

    [OnEnable]
    private void Subscribe()
    {
        Model.EventManager.AddAction(StateMachine.properties.SpinStartedEvent, ResetColor);
        Model.EventManager.AddAction<SlotItemSO>(StateMachine.properties.SpinFullStopEvent, SetColor);
    }

    [OnDisable]
    private void Unsubscribe()
    {
        Model.EventManager.RemoveAction(StateMachine.properties.SpinStartedEvent, ResetColor);
        Model.EventManager.RemoveAction<SlotItemSO>(StateMachine.properties.SpinFullStopEvent, SetColor);
    }

    public void ResetColor()
    {
        colorPath?.StopPath();

        Color startColor = bg.color;
        colorPath = CreateNewPath()
            .EasingQuadEaseIn(ResetTime, 0f, 1f, delta => {
                bg.color = Color.Lerp(startColor, NormalColor, delta);
                });
    }
    public void SetColor(SlotItemSO item)
    {
        colorPath?.StopPath();

        Color startColor = bg.color;
        colorPath = CreateNewPath()
            .EasingQuadEaseOut(RewardTime, 0f, 1f, delta => {
                bg.color = Color.Lerp(startColor, item.ItemColor, delta);
            });
    }
}
