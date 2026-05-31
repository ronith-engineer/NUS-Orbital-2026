using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private ItemData itemData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (InventoryManager.Instance.AddItem(itemData))
            {
                Debug.Log("Auto picked up: " + itemData.itemName);
                Destroy(gameObject);
            }
        }
    }
}