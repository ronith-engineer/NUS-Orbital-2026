using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private ItemData itemData;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private Pistol pistol;
    [SerializeField] private Shotgun shotgun;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (InventoryManager.Instance.AddItem(itemData))
            {
                if (itemData.itemType == ItemData.ItemType.Gun)
                {
                    weaponManager.AddWeapon(pistol);
                }
                else if (itemData.itemType == ItemData.ItemType.Shotgun)
                {
                    weaponManager.AddWeapon(shotgun);
                }
                Debug.Log("Auto picked up: " + itemData.itemName);
                Destroy(gameObject);
            }
        }
    }
}