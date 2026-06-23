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
    private Transform originalParent;
    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = itemIcon.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = itemIcon.gameObject.AddComponent<CanvasGroup>();
    }

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



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty() && actionButtons != null)
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

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
    public void ResetIcon()
    {
        itemIcon.transform.SetParent(transform);
        itemIcon.transform.localPosition = Vector3.zero;
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