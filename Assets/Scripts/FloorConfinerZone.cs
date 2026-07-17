using UnityEngine;

public class FloorConfinerZone : MonoBehaviour
{
    private Collider2D floorBounds;

    private void Awake()
    {
        floorBounds = GetComponent<PolygonCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraManager.Instance.SwitchConfiner(floorBounds);
        }
    }
}
