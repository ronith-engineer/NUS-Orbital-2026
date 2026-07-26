using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WeaponHUD : MonoBehaviour
{
    private Weapon weapon;

    [SerializeField] private ItemData.ItemType weaponType;

    [SerializeField] private TextMeshProUGUI ammoCounter;

    private void OnEnable()
    {
        weapon = InventoryManager.Instance.GetSpawnedWeapon(weaponType);
    }

    void Update()
    {
        if (weapon == null) return;  
        ammoCounter.text = $"{weapon.currentAmmo} | {weapon.reserveAmmo}";
    }
}
