using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text countText;

    private ItemData currentItem;

    
    public bool IsEmpty() => currentItem == null;

    
    public void SetItem(ItemData item)
    {
        currentItem = item;
        itemIcon.sprite = item.icon;
        itemIcon.color = Color.white;
        countText.text = "1";
    }

   
    public void ClearSlot()
    {
        currentItem = null;
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0); 
        countText.text = "";
    }

    
    public void OnSlotClicked()
    {
        if (currentItem != null)
            InventoryManager.Instance.UseItem(currentItem, this);
    }

    public ItemData GetItem() => currentItem;
}