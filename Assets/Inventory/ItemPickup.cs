using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private ItemData itemData;

    private bool playerNearby = false;

    private void Update()
    {
        
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
            PickUp();
    }

    private void PickUp()
    {
        
        if (InventoryManager.Instance.AddItem(itemData))
        {
            Debug.Log("Picked up: " + itemData.itemName);
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to pick up " + itemData.itemName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
            playerNearby = false;
    }
}