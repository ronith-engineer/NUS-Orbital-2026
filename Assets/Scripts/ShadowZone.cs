using UnityEngine;

public class ShadowZone : MonoBehaviour
{
    private const int ShadowZoneLayer = 13;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
                player.SetInShadow(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                int shadowMask = 1 << ShadowZoneLayer;
                Collider2D stillInShadow = Physics2D.OverlapPoint(player.transform.position, shadowMask);
                player.SetInShadow(stillInShadow != null);
            }
        }
    }
}