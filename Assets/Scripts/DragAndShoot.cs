using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    [SerializeField] private float power = 10f;
    private Rigidbody2D rb;

    [SerializeField] private Vector2 minPower;
    [SerializeField] private Vector2 maxPower;

    private TrajectoryLine trajectoryLine;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    public volatile bool isReleased = false;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        trajectoryLine = GetComponentInChildren<TrajectoryLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15f; //Set z value to 15 for drag UI to be visible
           
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15f; //Set z value to 15 for drag UI to be visible
            trajectoryLine.RenderLine(startPoint, currentPoint);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isReleased = true;
            rb.bodyType = RigidbodyType2D.Dynamic; // Set the Rigidbody to Dynamic to allow it to be affected by physics after being thrown
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15f; //Set z value to 15 for drag UI to be visible
            endPoint.z = 15f; //Set z value to 15 for drag UI to be visible
            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            rb.AddForce(force * power, ForceMode2D.Impulse);   
            trajectoryLine.Endline();
            rb.constraints = RigidbodyConstraints2D.None; //Unfreeze all constraints to allow movement after drag

            
        }
    }
}
