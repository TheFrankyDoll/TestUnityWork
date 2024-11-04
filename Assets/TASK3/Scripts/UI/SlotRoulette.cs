using AxGrid.Base;
using AxGrid.Path;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SlotRoulette : MonoBehaviourExt
{
    RectTransform _rectTransform;
    public RectTransform RectTransform {
        get {
            if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    [Header("References")]
    public GameObject CellPrefab;

    public Transform CellsParent;
    RouletteCell[] itemCells;

    [Header("Output")]
    public string rewardOutputField;

    [Header("Properties")]
    [SerializeField] int cellsCount = 5;
    [SerializeField] float cellHeight;
    [SerializeField] float cellSpacing;
    [Space(5)]
    [SerializeField] float maxSpeed;
    [Space(5)]
    [SerializeField] float rollbackDuration = 0.5f;

    CPath spinPath;
    CPath speedChangePath;
    CPath stopPath;

    SlotItemSO[] currentItems;

    float currentSpeed = 0f;
    float itemsOffset = 0f;
    float spinTime = 0f;

    private float CellInterval => cellHeight + cellSpacing;

    [OnEnable]
    private void Subscribe() {
        Model.EventManager.AddAction<float>(StateMachineT3.properties.SpinStartedEvent, Spin);
        Model.EventManager.AddAction<float>(StateMachineT3.properties.SpinSlowdownEvent, Slowdown);
        Model.EventManager.AddAction(StateMachineT3.properties.SpinStopEvent, FullStop);
    }

    [OnDisable]
    private void Unsubscribe() {
        Model.EventManager.RemoveAction<float>(StateMachineT3.properties.SpinStartedEvent, Spin);
        Model.EventManager.RemoveAction<float>(StateMachineT3.properties.SpinSlowdownEvent, Slowdown);
        Model.EventManager.RemoveAction(StateMachineT3.properties.SpinStopEvent, FullStop);
    }



    public void Init(IEnumerable<SlotItemSO> spinItems)
    {
        if (spinItems.Count() == 0) throw new System.ArgumentException($"No items in Enumerable - cannot init {GetType().Name}");
        currentItems = spinItems.ToArray();

        ClearCells();

        itemCells = new RouletteCell[cellsCount];
        for (int i = 0; i < cellsCount; i++)
        {
            var cell = Instantiate(CellPrefab, CellsParent).GetComponent<RouletteCell>();
            cell.Init(currentItems[Random.Range(0, currentItems.Length)]);

            float y = CellInterval * (i - (cellsCount / 2));
            cell.SetPosition(RectTransform.localPosition + new Vector3(0f, y));

            itemCells[i] = cell;
            cell.transform.SetAsFirstSibling();
        }
    }

    public void Spin(float accelerationDuration)
    {
        speedChangePath?.StopPath();

        if (accelerationDuration > 0f)
        {
            currentSpeed = 0f;
            speedChangePath = CreateNewPath()
                .EasingQuadEaseOut(accelerationDuration, 0f, maxSpeed,
                speed => currentSpeed = speed);
        }
        else currentSpeed = maxSpeed;

        spinTime = 0f;
        spinPath = CreateNewPath()
            .Add(context =>
            {
                float deltaTime = context.PathStartTimeF - spinTime;
                spinTime = context.PathStartTimeF;
                itemsOffset += (currentSpeed * deltaTime);
                if(itemsOffset >= CellInterval) RedrawEdgeItem();
                MoveItems(itemsOffset);
                return Status.OK;
            });
        spinPath.Loop = true;
    }

    public void Slowdown (float decelerationDuration)
    {
        speedChangePath?.StopPath();
        if (decelerationDuration > 0f)
        {
            speedChangePath = CreateNewPath()
                .EasingCircEaseOut(decelerationDuration, currentSpeed, 0f,
                speed =>
                {
                    currentSpeed = speed;
                });
        }
        else currentSpeed = 0f;
    }

    public void FullStop()
    {
        spinPath?.StopPath();
        speedChangePath?.StopPath();

        int selectedIndex = CellsParent.childCount / 2;
        float rollbackOffset = 0f;
        if (itemsOffset > (CellInterval / 2))
        {
            selectedIndex--;
            rollbackOffset = CellInterval;
        }

        SlotItemSO currentReward = CellsParent.GetChild(selectedIndex).GetComponent<RouletteCell>().item;
        if (!string.IsNullOrWhiteSpace(rewardOutputField)) Model.Set(rewardOutputField, currentReward);

        stopPath = CreateNewPath()
            .EasingQuadEaseIn(
                rollbackDuration,
                itemsOffset,
                rollbackOffset,
                offset => {
                    MoveItems(offset);
                    itemsOffset = offset;
                })
            .Action(() =>
            {
                Model.EventManager.Invoke(StateMachineT3.properties.SpinFullStopEvent, currentReward);
            });
    }


    private void MoveItems(float offset)
    {
        int countHalf = this.cellsCount / 2;
        foreach (var item in itemCells)
        {
            float y = (CellInterval * (item.transform.GetSiblingIndex() - countHalf)) + offset;
            item.SetPosition(RectTransform.localPosition + new Vector3(0f, -y));
        }
    }

    private void RedrawEdgeItem()
    {
        itemsOffset -= CellInterval;
        var edgeCell = CellsParent.GetChild(CellsParent.childCount-1).GetComponent<RouletteCell>();
        edgeCell.Init(currentItems[Random.Range(0, currentItems.Length)]);
        edgeCell.transform.SetSiblingIndex(0);
        
    }

    private void ClearCells()
    {
        foreach (Transform child in CellsParent) Destroy(child.gameObject);
        itemCells = null;
        itemsOffset = 0f;
    }
}