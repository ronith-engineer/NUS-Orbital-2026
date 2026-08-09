using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public class InventoryManager : MonoBehaviour, ICloseableUI
{
    public static InventoryManager Instance;
    public event Action OnEquippedItemChanged;
    [SerializeField] private WeaponManager weaponManager;

    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlot[] slots;

    [Header("Parents")]
    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform playerTransform;

    [Header("Held Prefabs")]
    [SerializeField] private GameObject gunHeldPrefab;
    [SerializeField] private GameObject shotgunHeldPrefab;
    [SerializeField] private GameObject knifeHeldPrefab;
    [SerializeField] private GameObject molotovHeldPrefab;
    [SerializeField] private GameObject grenadeHeldPrefab;

    [Header("Ground Pickup Prefabs")]
    [SerializeField] private GameObject gunPickupPrefab;
    [SerializeField] private GameObject shotgunPickupPrefab;
    [SerializeField] private GameObject knifePickupPrefab;
    [SerializeField] private GameObject molotovPickupPrefab;
    [SerializeField] private GameObject grenadePickupPrefab;
    [SerializeField] private GameObject medkitPickupPrefab;
    [SerializeField] private GameObject bandagePickupPrefab;
    [SerializeField] private GameObject pistolAmmoPickupPrefab;
    [SerializeField] private GameObject shotgunAmmoPickupPrefab;
    [SerializeField] private GameObject alcoholPickupPrefab;
    [SerializeField] private GameObject ragsPickupPrefab;
    [SerializeField] private GameObject metalScrapPickupPrefab;
    [SerializeField] private GameObject keycardPickup;

    [Header("Drop Settings")]
    [SerializeField] private float dropOffsetX = 2f;

    [Header("Slow Motion")]
    [SerializeField] private float slowMotionScale = 0.1f;
    [SerializeField] private bool useSlowMotion = true;

    private bool isOpen = false;
    private ItemData equippedItem;

    private Dictionary<ItemData.ItemType, GameObject> spawnedObjects = new Dictionary<ItemData.ItemType, GameObject>();
    private Dictionary<ItemData.ItemType, Weapon> spawnedWeapons = new Dictionary<ItemData.ItemType, Weapon>();

    private void Awake()
    {
        Instance = this;
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            OpenUI();
    }

    private ItemData.ItemType NormalizeType(ItemData.ItemType type)
    {
        if (type == ItemData.ItemType.MakeshiftKnife)
            return ItemData.ItemType.Knife;
        return type;
    }

    public void OpenUI()
    {
        if (isOpen) return; 
        if (!MenuManager.Instance.RegisterOpenUI(this))
            return; 

        isOpen = true;
        SetSlowMotion(true);
        inventoryPanel.SetActive(true);
        inventoryPanel.transform.localScale = Vector3.zero;
        StartCoroutine(ScaleInventory(Vector3.one));
    }

    public void CloseUI()
    {
        if (!isOpen) return;
        isOpen = false;
        SetSlowMotion(false);
        StartCoroutine(ScaleInventory(Vector3.zero));
        StartCoroutine(HideAfterScale());
        MenuManager.Instance.UnregisterOpenUI(this);
    }


    private void SetSlowMotion(bool slow)
    {
        if (!useSlowMotion) return;

        if (slow)
        {
            Time.timeScale = slowMotionScale;
            Time.fixedDeltaTime = 0.02f * slowMotionScale;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
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
        if (item.isStackable)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.CanStack(item))
                {
                    slot.AddCount(1);
                    return true;
                }
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(item);
                SpawnHeldObject(item);
                return true;
            }
        }

        Debug.Log("Inventory full!");
        return false;
    }

    private void SpawnHeldObject(ItemData item)
    {
        ItemData.ItemType key = NormalizeType(item.itemType);
        Debug.Log("SpawnHeldObject running for: " + key);

        if (key == ItemData.ItemType.Molotov || key == ItemData.ItemType.Grenade)
            return;

        if (spawnedObjects.ContainsKey(key)) return;

        GameObject prefab = null;
        Transform parent = null;

        switch (key)
        {
            case ItemData.ItemType.Gun:
                prefab = gunHeldPrefab;
                parent = aimTransform;
                break;
            case ItemData.ItemType.Shotgun:
                prefab = shotgunHeldPrefab;
                parent = aimTransform;
                break;
            case ItemData.ItemType.Knife:
                prefab = knifeHeldPrefab;
                parent = aimTransform;
                break;
            default:
                return;
        }

        Debug.Log("Prefab null: " + (prefab == null) + " | Parent null: " + (parent == null));

        if (prefab == null || parent == null) return;

        GameObject spawned = Instantiate(prefab, parent);
        spawnedObjects[key] = spawned;

        Weapon weapon = spawned.GetComponent<Weapon>();
        Debug.Log("Weapon component found on " + key + ": " + (weapon != null));

        if (weapon != null)
        {
            weaponManager.AddWeapon(weapon);
            spawnedWeapons[key] = weapon;
        }

        spawned.SetActive(false);
    }

    public void EquipItem(ItemData item, InventorySlot slot)
    {
        ItemData.ItemType key = NormalizeType(item.itemType);
        Debug.Log("EquipItem called for: " + key + " | in spawnedWeapons: " + spawnedWeapons.ContainsKey(key));

        switch (key)
        {
            case ItemData.ItemType.KeyCard:
                equippedItem = item;
                break;
            case ItemData.ItemType.Gun:
                SelectSpawnedWeapon(key);
                equippedItem = item;
                OnEquippedItemChanged?.Invoke();
                break;
            case ItemData.ItemType.Shotgun:
                SelectSpawnedWeapon(key);
                equippedItem = item;
                OnEquippedItemChanged?.Invoke();
                break;
            case ItemData.ItemType.Knife:
                ShowOnlyHeld(key);
                equippedItem = item;
                OnEquippedItemChanged?.Invoke();
                break;
            case ItemData.ItemType.Molotov:
                SpawnThrowable(ItemData.ItemType.Molotov, molotovHeldPrefab);
                equippedItem = item;
                OnEquippedItemChanged?.Invoke();
                break;
            case ItemData.ItemType.Grenade:
                SpawnThrowable(ItemData.ItemType.Grenade, grenadeHeldPrefab);
                equippedItem = item;
                OnEquippedItemChanged?.Invoke();
                break;
            case ItemData.ItemType.Medkit:
                HealPlayer();
                slot.RemoveOne();
                break;
            case ItemData.ItemType.Bandage:
                HealPlayer();
                slot.RemoveOne();
                break;
            case ItemData.ItemType.PistolAmmo:
                foreach (Weapon weapon in weaponManager.ownedWeapons)
                {
                    if (weapon is Pistol)
                    {
                        weapon.AddToReserveAmmo(4);
                        slot.RemoveOne();
                        break;
                    }
                }
                break;
            case ItemData.ItemType.ShotgunAmmo:
                foreach (Weapon weapon in weaponManager.ownedWeapons)
                {
                    if (weapon is Shotgun)
                    {
                        weapon.AddToReserveAmmo(3);
                        slot.RemoveOne();
                        break;
                    }
                }
                break;
        }
    }

    private void SpawnThrowable(ItemData.ItemType type, GameObject prefab)
    {
        HideAllHeld();

        if (spawnedObjects.ContainsKey(type) && spawnedObjects[type] != null)
        {
            spawnedObjects[type].SetActive(true);
            return;
        }

        if (prefab == null || playerTransform == null) return;

        GameObject spawned = Instantiate(prefab, playerTransform);
        spawnedObjects[type] = spawned;
        spawned.SetActive(true);
    }

    private void SelectSpawnedWeapon(ItemData.ItemType type)
    {
        HideAllHeld();
        Debug.Log("SelectSpawnedWeapon for " + type + " | key present: " + spawnedWeapons.ContainsKey(type));
        if (spawnedWeapons.ContainsKey(type))
            weaponManager.SelectWeapon(spawnedWeapons[type]);
    }

    private void ShowOnlyHeld(ItemData.ItemType type)
    {
        HideAllHeld();
        if (spawnedObjects.ContainsKey(type))
            spawnedObjects[type].SetActive(true);
    }

    private void HideAllHeld()
    {
        foreach (var pair in spawnedObjects)
        {
            if (pair.Value != null)
                pair.Value.SetActive(false);
        }
    }

    private void HealPlayer()
    {
        Player player = FindAnyObjectByType<Player>();
        if (player != null)
            player.Heal();
    }

    public void NotifyThrowableUsed(ItemData.ItemType type)
    {
        ItemData.ItemType key = NormalizeType(type);

        if (spawnedObjects.ContainsKey(key))
            spawnedObjects.Remove(key);

        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.GetItem().itemType == type)
            {
                slot.RemoveOne();
                return;
            }
        }
    }

    public void DropItem(ItemData item, InventorySlot slot)
    {
        ItemData.ItemType key = NormalizeType(item.itemType);
        bool isLastOne = slot.GetCount() <= 1;

        if (isLastOne)
        {
            if (spawnedWeapons.ContainsKey(key))
            {
                weaponManager.RemoveWeapon(spawnedWeapons[key]);
                spawnedWeapons.Remove(key);
            }

            if (spawnedObjects.ContainsKey(key))
            {
                Destroy(spawnedObjects[key]);
                spawnedObjects.Remove(key);
            }

            if (equippedItem != null && NormalizeType(equippedItem.itemType) == key)
            {
                equippedItem = null;
                OnEquippedItemChanged?.Invoke();
            }
        }

        GameObject pickupPrefab = GetPickupPrefab(item.itemType);

        if (pickupPrefab != null && playerTransform != null)
        {
            Vector3 dropPosition = new Vector3(
                playerTransform.position.x + dropOffsetX,
                playerTransform.position.y,
                0);
            Instantiate(pickupPrefab, dropPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Drop failed for " + item.itemType + ". Pickup null: " + (pickupPrefab == null));
        }

        slot.RemoveOne();
    }

    private GameObject GetPickupPrefab(ItemData.ItemType type)
    {
        switch (type)
        {
            case ItemData.ItemType.Gun: return gunPickupPrefab;
            case ItemData.ItemType.Shotgun: return shotgunPickupPrefab;
            case ItemData.ItemType.Knife: return knifePickupPrefab;
            case ItemData.ItemType.MakeshiftKnife: return knifePickupPrefab;
            case ItemData.ItemType.Molotov: return molotovPickupPrefab;
            case ItemData.ItemType.Grenade: return grenadePickupPrefab;
            case ItemData.ItemType.Medkit: return medkitPickupPrefab;
            case ItemData.ItemType.Bandage: return bandagePickupPrefab;
            case ItemData.ItemType.PistolAmmo: return pistolAmmoPickupPrefab;
            case ItemData.ItemType.ShotgunAmmo: return shotgunAmmoPickupPrefab;
            case ItemData.ItemType.Alcohol: return alcoholPickupPrefab;
            case ItemData.ItemType.Rags: return ragsPickupPrefab;
            case ItemData.ItemType.MetalScrap: return metalScrapPickupPrefab;
            default: return null;
        }
    }

    public void ConsumeOne(InventorySlot slot)
    {
        slot.RemoveOne();
    }

    public void UseItem(ItemData item, InventorySlot slot)
    {
        EquipItem(item, slot);
    }

    public ItemData GetEquippedItem() => equippedItem;

    public Weapon GetSpawnedWeapon(ItemData.ItemType type)
    {
        ItemData.ItemType key = NormalizeType(type);
        spawnedWeapons.TryGetValue(key, out Weapon weapon);
        return weapon;
    }

    public Knife GetSpawnedKnife(ItemData.ItemType type)
    {
        ItemData.ItemType key = NormalizeType(type);
        if (spawnedObjects.TryGetValue(key, out GameObject obj) && obj != null)
            return obj.GetComponent<Knife>();
        return null;
    }




}


