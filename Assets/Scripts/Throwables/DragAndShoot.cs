using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float power;
    [SerializeField] private ItemData.ItemType throwableType;
    private TrajectoryLine trajectoryLine;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;
    Vector3 currentPoint;

    private bool clickedWithinRadius;
    private Player player;

    public volatile bool isReleased = false;
    [SerializeField] private float maxDragRadius;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        trajectoryLine = GetComponentInChildren<TrajectoryLine>();
        clickedWithinRadius = false;
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (clickedWithinRadius)
        {
            startPoint = transform.position;
            HandleDragAndShoot();
        }
    }

    public void SetClickWithinRadius(bool withinRadius)
    {
        clickedWithinRadius = withinRadius;
        startPoint = transform.position;
        startPoint.z = 15f;
    }

    private void HandleDragAndShoot()
    {
        player.EnableMovementAndJump(false);
        if (Input.GetMouseButton(0))
        {
            currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15f;
            Vector3 rawDragVector = currentPoint - startPoint;
            float magDragVector = rawDragVector.magnitude;
            if (magDragVector > maxDragRadius)
            {
                Vector3 correctDragVector = (rawDragVector.normalized * maxDragRadius);
                currentPoint = correctDragVector + startPoint;
            }
            trajectoryLine.RenderLine(startPoint, currentPoint);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isReleased = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            endPoint = currentPoint;
            force = startPoint - endPoint;
            rb.AddForce(force * power, ForceMode2D.Impulse);
            trajectoryLine.Endline();
            rb.constraints = RigidbodyConstraints2D.None;

            if (InventoryManager.Instance != null)
                InventoryManager.Instance.NotifyThrowableUsed(throwableType);

            clickedWithinRadius = false;
            player.EnableMovementAndJump(true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            trajectoryLine.Endline();
            clickedWithinRadius = false;
            player.EnableMovementAndJump(true);
        }
    }
}