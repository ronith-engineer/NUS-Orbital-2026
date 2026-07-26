using UnityEngine;
using UnityEngine.PlayerLoop;

public class WeaponUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pistolUI;
    [SerializeField] private GameObject knifeUI;
    [SerializeField] private GameObject shotgunUI;
    [SerializeField] private GameObject molotovUI;
    [SerializeField] private GameObject grenadeUI;

    [Header("Weapon References")]
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject molotov;
    [SerializeField] private GameObject grenade;

    private void OnEnable()
    {
        InventoryManager.Instance.OnEquippedItemChanged += UpdateUI;
        UpdateUI();
    }
    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnEquippedItemChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        ItemData equipped = InventoryManager.Instance.GetEquippedItem();

        pistolUI.SetActive(false);
        knifeUI.SetActive(false);
        shotgunUI.SetActive(false);
        molotovUI.SetActive(false);
        grenadeUI.SetActive(false);

        if (equipped == null) return;

        switch (equipped.itemType)
        {
            case ItemData.ItemType.Gun:
                pistolUI.SetActive(true);
                break;
            case ItemData.ItemType.Knife:
            case ItemData.ItemType.MakeshiftKnife:
                knifeUI.SetActive(true);
                break;
            case ItemData.ItemType.Shotgun:
                shotgunUI.SetActive(true);
                break;
            case ItemData.ItemType.Molotov:
                molotovUI.SetActive(true);
                break;
            case ItemData.ItemType.Grenade:
                grenadeUI.SetActive(true);
                break;
        }
    }

}
