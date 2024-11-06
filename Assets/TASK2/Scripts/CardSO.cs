using UnityEngine;

[CreateAssetMenu(fileName = nameof(CardSO), menuName = "ScriptableObjects/" + nameof(CardSO))]
public class CardSO : ScriptableObject
{
    public string cardName;
    public Sprite itemSprite;
}
