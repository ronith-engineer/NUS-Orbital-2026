using System.Collections.Generic;
using UnityEngine;
using System.Collections;


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
        if (isOpen)
        {
            inventoryPanel.SetActive(true);
            inventoryPanel.transform.localScale = Vector3.zero;
            StartCoroutine(ScaleInventory(Vector3.one));
        }
        else
        {
            StartCoroutine(ScaleInventory(Vector3.zero));
            StartCoroutine(HideAfterScale());
        }
    }

    private IEnumerator HideAfterScale()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        inventoryPanel.SetActive(false);
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
    private IEnumerator ScaleInventory(Vector3 targetScale)
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 startScale = inventoryPanel.transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            inventoryPanel.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        inventoryPanel.transform.localScale = targetScale;
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