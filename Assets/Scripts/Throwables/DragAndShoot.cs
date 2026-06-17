using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float power;
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
        clickedWithinRadius = false; // Initialize the flag to false at the start
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
    public void SetClickWithinRadius(bool withinRadius) // Method to set the flag from ClickRadiusRestriction script
    {
        clickedWithinRadius = withinRadius;
        startPoint = transform.position;
        startPoint.z = 15f; //Set z value to 15 for drag UI to be visible
    }

    private void HandleDragAndShoot()
    {
        player.EnableMovementAndJump(false); // Disable player movement and jumping while dragging
        if (Input.GetMouseButton(0))
        {
            // Calculate the current point based on the mouse position, clamping it within the defined min and max power limits
            currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15f; //Set z value to 15 for drag UI to be visible
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
            rb.bodyType = RigidbodyType2D.Dynamic; // Set the Rigidbody to Dynamic to allow it to be affected by physics after being thrown
            endPoint = currentPoint;
            force = startPoint - endPoint;
            rb.AddForce(force * power , ForceMode2D.Impulse);
            trajectoryLine.Endline();
            rb.constraints = RigidbodyConstraints2D.None; //Unfreeze all constraints to allow movement after drag

            clickedWithinRadius = false; // Reset the flag to allow for the next drag and shoot action
            player.EnableMovementAndJump(true); // Re-enable player movement and jumping after shooting
        }
        
        if (Input.GetMouseButtonDown(1)) // Right-click to cancel the drag and shoot action
        {
            trajectoryLine.Endline();
            clickedWithinRadius = false; // Reset the flag to allow for the next drag and shoot action
            player.EnableMovementAndJump(true); // Re-enable player movement and jumping after canceling

        }
    }
}
