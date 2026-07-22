using UnityEngine;

public class PartsPickup : MonoBehaviour
{
    [SerializeField] private int partsAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PartsManager.Instance.AddParts(partsAmount);
            Destroy(gameObject);
        }
    }
}
