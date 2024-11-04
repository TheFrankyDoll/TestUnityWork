using UnityEngine;

[CreateAssetMenu(fileName = nameof(SlotItemSO), menuName = "ScriptableObjects/" + nameof(SlotItemSO))]
public class SlotItemSO : ScriptableObject
{
    public Sprite ItemSprite;

    public Color ItemColor;
}
