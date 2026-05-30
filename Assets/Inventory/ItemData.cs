using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    [Header("Item Stats")]
    public int healAmount = 0;

    public enum ItemType
    {
        Gun,
        Knife,
        Medkit
    }
}