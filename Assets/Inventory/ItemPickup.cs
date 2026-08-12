using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private ItemData itemData;

    private bool isPickedUp = false;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isPickedUp) return;

        if (collision.CompareTag("Player"))
        {
            if (InventoryManager.Instance.AddItem(itemData))
            {
                isPickedUp = true;
                Destroy(gameObject);
            }
        }
    }
}