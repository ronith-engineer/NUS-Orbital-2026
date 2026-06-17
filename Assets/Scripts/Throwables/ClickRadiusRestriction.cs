using System;
using UnityEngine;

public class ClickRadiusRestriction : MonoBehaviour
{
    [SerializeField] private float clickRadius;
    private float squaredClickRadius;
    private DragAndShoot dragAndShoot;


    private void Start()
    {
        squaredClickRadius = clickRadius * clickRadius; // Precompute the squared radius for distance checks
        dragAndShoot = GetComponent<DragAndShoot>();
    }

    private void Update()
    {
        CheckClick();
    }

    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f; // Set z to 0 for 2D adjustment
            // Check if the click is within the specified radius
            if (IsWithinClickRadius(mouseWorldPosition))
            {
                dragAndShoot.SetClickWithinRadius(true);
            }

        }
    }

    private bool IsWithinClickRadius(Vector3 mouseWorldPosition)
    {
        // Calculate the squared distance between the mouse position and the object's position
        float squaredDistance = (mouseWorldPosition - transform.position).sqrMagnitude;
        // Compare with the squared click radius
        return squaredDistance <= squaredClickRadius;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere in the editor to visualize the click radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, clickRadius);
    }
}
