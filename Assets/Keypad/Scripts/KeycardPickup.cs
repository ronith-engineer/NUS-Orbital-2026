using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            KeycardManager.Instance.CollectKeycard();
            Destroy(gameObject);
        }
    }
}