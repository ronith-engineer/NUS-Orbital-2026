using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlot[] slots;

    private bool isOpen = false;
    private ItemData equippedItem;

    private void Awake()
    {
        Instance = this;
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ToggleInventory();
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
    }

    public bool AddItem(ItemData item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(item);
                return true;
            }
        }
        Debug.Log("Inventory full!");
        return false;
    }

    public void RemoveItem(InventorySlot slot)
    {
        slot.ClearSlot();
    }

    public void UseItem(ItemData item, InventorySlot slot)
    {
        switch (item.itemType)
        {
            case ItemData.ItemType.Medkit:
                Player player = FindAnyObjectByType<Player>();
                if (player != null)
                {
                    player.Heal(item.healAmount);
                    RemoveItem(slot);
                }
                break;
            case ItemData.ItemType.Gun:
                equippedItem = item;
                Debug.Log("Gun equipped!");
                break;
            case ItemData.ItemType.Knife:
                equippedItem = item;
                Debug.Log("Knife equipped!");
                break;
        }
    }

    public ItemData GetEquippedItem() => equippedItem;
}