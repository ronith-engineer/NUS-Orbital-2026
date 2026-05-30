using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text countText;

    private ItemData currentItem;

    // Returns true if slot has no item
    public bool IsEmpty() => currentItem == null;

    // Called when item picked up — fills the slot
    public void SetItem(ItemData item)
    {
        currentItem = item;
        itemIcon.sprite = item.icon;
        itemIcon.color = Color.white; // makes icon visible
        countText.text = "1";
    }

    // Called when item used/dropped — empties the slot
    public void ClearSlot()
    {
        currentItem = null;
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0); // makes icon invisible
        countText.text = "";
    }

    // Called when player clicks the slot
    public void OnSlotClicked()
    {
        if (currentItem != null)
            InventoryManager.Instance.UseItem(currentItem, this);
    }

    public ItemData GetItem() => currentItem;
}