using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlot[] slots;

    [Header("Player References")]
    [SerializeField] private GameObject pistolObject;
    [SerializeField] private GameObject knifeObject;
    [SerializeField] private GameObject shotgunObject;
    [SerializeField] private GameObject molotovObject;
    [SerializeField] private GameObject grenadeObject;
    [SerializeField] private Transform playerTransform;

    [Header("Drop Prefabs")]
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private GameObject shotgunPrefab;
    [SerializeField] private GameObject molotovPrefab;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GameObject medkitPrefab;

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

    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        inventoryPanel.transform.localScale = Vector3.one;
    }

    private IEnumerator HideAfterScale()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        inventoryPanel.SetActive(false);
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

    public void EquipItem(ItemData item, InventorySlot slot)
    {
        switch (item.itemType)
        {
            case ItemData.ItemType.Gun:
                pistolObject.SetActive(true);
                knifeObject.SetActive(false);
                shotgunObject.SetActive(false);
                molotovObject.SetActive(false);
                grenadeObject.SetActive(false);
                equippedItem = item;
                Debug.Log("Gun equipped!");
                break;
            case ItemData.ItemType.Knife:
                knifeObject.SetActive(true);
                pistolObject.SetActive(false);
                shotgunObject.SetActive(false);
                molotovObject.SetActive(false);
                grenadeObject.SetActive(false);
                equippedItem = item;
                Debug.Log("Knife equipped!");
                break;
            case ItemData.ItemType.Shotgun:
                shotgunObject.SetActive(true);
                pistolObject.SetActive(false);
                knifeObject.SetActive(false);
                molotovObject.SetActive(false);
                grenadeObject.SetActive(false);
                equippedItem = item;
                Debug.Log("Shotgun equipped!");
                break;
            case ItemData.ItemType.Molotov:
                molotovObject.SetActive(true);
                pistolObject.SetActive(false);
                knifeObject.SetActive(false);
                shotgunObject.SetActive(false);
                grenadeObject.SetActive(false);
                equippedItem = item;
                Debug.Log("Molotov equipped!");
                break;
            case ItemData.ItemType.Grenade:
                grenadeObject.SetActive(true);
                molotovObject.SetActive(false);
                pistolObject.SetActive(false);
                knifeObject.SetActive(false);
                shotgunObject.SetActive(false);
                equippedItem = item;
                Debug.Log("Grenade equipped!");
                break;
            case ItemData.ItemType.Medkit:
                Player player = FindAnyObjectByType<Player>();
                if (player != null)
                    player.Heal();
                slot.ClearSlot();
                Debug.Log("Medkit used!");
                break;
            case ItemData.ItemType.Bandage:
                Player bandagePlayer = FindAnyObjectByType<Player>();
                if (bandagePlayer != null)
                    bandagePlayer.Heal();
                slot.ClearSlot();
                Debug.Log("Bandage used!");
                break;
            case ItemData.ItemType.MakeshiftKnife:
                knifeObject.SetActive(true);
                pistolObject.SetActive(false);
                shotgunObject.SetActive(false);
                molotovObject.SetActive(false);
                grenadeObject.SetActive(false);
                equippedItem = item;
                Debug.Log("Makeshift Knife equipped!");
                break;
        }
    }

    public void DropItem(ItemData item, InventorySlot slot)
    {
        if (item.itemType == ItemData.ItemType.Gun)
            Destroy(pistolObject);
        if (item.itemType == ItemData.ItemType.Knife)
            Destroy(knifeObject);
        if (item.itemType == ItemData.ItemType.Shotgun)
            Destroy(shotgunObject);
        if (item.itemType == ItemData.ItemType.Molotov)
            Destroy(molotovObject);
        if (item.itemType == ItemData.ItemType.Grenade)
            Destroy(grenadeObject);
        if (item.itemType == ItemData.ItemType.MakeshiftKnife)
            knifeObject.SetActive(false);


        GameObject prefabToSpawn = null;
        switch (item.itemType)
        {
            case ItemData.ItemType.Gun:
                prefabToSpawn = gunPrefab;
                break;
            case ItemData.ItemType.Knife:
                prefabToSpawn = knifePrefab;
                break;
            case ItemData.ItemType.Shotgun:
                prefabToSpawn = shotgunPrefab;
                break;
            case ItemData.ItemType.Molotov:
                prefabToSpawn = molotovPrefab;
                break;
            case ItemData.ItemType.Grenade:
                prefabToSpawn = grenadePrefab;
                break;
            case ItemData.ItemType.Medkit:
                prefabToSpawn = medkitPrefab;
                break;
            case ItemData.ItemType.Bandage:
                prefabToSpawn = medkitPrefab;
                break;
            case ItemData.ItemType.MakeshiftKnife:
                prefabToSpawn = knifePrefab;
                break;
        }

        if (prefabToSpawn != null)
        {
            Vector3 dropPosition = new Vector3(playerTransform.position.x, playerTransform.position.y - 1f, 0);
            Instantiate(prefabToSpawn, dropPosition, Quaternion.identity);
        }

        slot.ClearSlot();
        Debug.Log("Dropped: " + item.itemName);
    }

    public void UseItem(ItemData item, InventorySlot slot)
    {
        EquipItem(item, slot);
    }

    public ItemData GetEquippedItem() => equippedItem;
}