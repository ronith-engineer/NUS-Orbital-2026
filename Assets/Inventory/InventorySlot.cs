using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject actionButtons;

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
        if (actionButtons != null)
            actionButtons.SetActive(false);
    }

    // Mouse hovers over slot — show buttons
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty() && actionButtons != null)
            actionButtons.SetActive(true);
    }

    // Mouse leaves slot — hide buttons
    public void OnPointerExit(PointerEventData eventData)
    {
        if (actionButtons != null)
            actionButtons.SetActive(false);
    }

    public void OnEquipClicked()
    {
        if (currentItem != null)
            InventoryManager.Instance.EquipItem(currentItem);
    }

    public void OnDropClicked()
    {
        if (currentItem != null)
            InventoryManager.Instance.DropItem(currentItem, this);
    }

    public ItemData GetItem() => currentItem;
}