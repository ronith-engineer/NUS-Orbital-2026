using NUnit.Framework.Interfaces;
using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private ItemData itemData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            if (InventoryManager.Instance.AddItem(itemData))
            {
                KeycardManager.Instance.CollectKeycard();
                Destroy(gameObject);
            }
            
        }
    }
}