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

    [Header("Stacking")]
    public bool isStackable = false;
    public int maxStack = 5;


    [Header("Keycard")]
    public int gateID = 0;


    public enum ItemType
    {
        Gun,
        Knife,
        Shotgun,
        Molotov,
        Grenade,
        Medkit,
        Alcohol,
        Rags,
        MetalScrap,
        Bandage,
        MakeshiftKnife,
        PistolAmmo,
        ShotgunAmmo,
        KeyCard
    }
}