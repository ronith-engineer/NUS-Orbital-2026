using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private ItemData itemData;

    private bool playerNearby = false;

    private void Update()
    {
        // When player is nearby and presses E
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
            PickUp();
    }

    private void PickUp()
    {
        // Try to add item to inventory
        if (InventoryManager.Instance.AddItem(itemData))
        {
            Debug.Log("Picked up: " + itemData.itemName);
            Destroy(gameObject); // remove item from ground
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player walked near item
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to pick up " + itemData.itemName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Player walked away
        if (collision.CompareTag("Player"))
            playerNearby = false;
    }
}