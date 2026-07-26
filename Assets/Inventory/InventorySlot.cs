using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject actionButtons;

    private ItemData currentItem;
    private int count = 0;
    private Transform originalParent;
    private Vector3 originalPosition;
    private int originalSiblingIndex;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = itemIcon.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = itemIcon.gameObject.AddComponent<CanvasGroup>();
    }

    public bool IsEmpty() => currentItem == null;

    public int GetCount() => count;

    public bool CanStack(ItemData item)
    {
        if (currentItem == null) return false;
        if (!item.isStackable) return false;
        if (currentItem.itemType != item.itemType) return false;
        return count < item.maxStack;
    }

    public void SetItem(ItemData item)
    {
        currentItem = item;
        count = 1;
        itemIcon.sprite = item.icon;
        itemIcon.color = Color.white;
        UpdateCountText();
    }

    public void AddCount(int amount)
    {
        count += amount;
        UpdateCountText();
    }

    public void RemoveOne()
    {
        count--;
        if (count <= 0)
            ClearSlot();
        else
            UpdateCountText();
    }

    private void UpdateCountText()
    {
        if (countText == null) return;

        if (currentItem != null && currentItem.isStackable && count > 1)
            countText.text = count.ToString();
        else
            countText.text = "";
    }

    public void ClearSlot()
    {
        currentItem = null;
        count = 0;
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        if (countText != null)
            countText.text = "";
        if (actionButtons != null)
            actionButtons.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEmpty()) return;
        if (actionButtons == null) return;

        if (currentItem.itemType == ItemData.ItemType.Alcohol ||
            currentItem.itemType == ItemData.ItemType.Rags ||
            currentItem.itemType == ItemData.ItemType.MetalScrap)
            return;

        actionButtons.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (actionButtons != null)
            actionButtons.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEmpty()) return;

        originalParent = itemIcon.transform.parent;
        originalPosition = itemIcon.transform.position;
        originalSiblingIndex = itemIcon.transform.GetSiblingIndex();

        itemIcon.transform.SetParent(GetComponentInParent<Canvas>().transform);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;

        if (actionButtons != null)
            actionButtons.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsEmpty()) return;

        itemIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemIcon.transform.SetParent(transform);
        itemIcon.transform.localPosition = Vector3.zero;
        itemIcon.transform.SetSiblingIndex(originalSiblingIndex);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void ResetIcon()
    {
        itemIcon.transform.SetParent(transform);
        itemIcon.transform.localPosition = Vector3.zero;
        itemIcon.transform.SetSiblingIndex(originalSiblingIndex);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot fromSlot = eventData.pointerDrag.GetComponent<InventorySlot>();

        if (fromSlot == null || fromSlot == this) return;
        if (fromSlot.IsEmpty()) return;

        if (CraftingManager.Instance != null)
        {
            if (CraftingManager.Instance.TryCraft(fromSlot, this))
                return;
        }
    }

    public void OnEquipClicked()
    {
        if (currentItem != null)
            InventoryManager.Instance.EquipItem(currentItem, this);
    }

    public void OnDropClicked()
    {
        if (currentItem != null)
            InventoryManager.Instance.DropItem(currentItem, this);
    }

    public ItemData GetItem() => currentItem;
}