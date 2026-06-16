using UnityEngine;

public class MolotovProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float angle;
    private DragAndShoot dragAndShoot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // Freeze all constraints to prevent movement until the projectile is thrown
        rb.bodyType = RigidbodyType2D.Kinematic; // Set the Rigidbody to Kinematic to prevent it from being affected by physics until thrown
        boxCollider = GetComponent<BoxCollider2D>();
        dragAndShoot = GetComponent<DragAndShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dragAndShoot.isReleased) // If the projectile has been released, allow it to rotate based on its velocity
        {
            transform.parent = null; // Detach from parent to allow independent movement
            rb.constraints = RigidbodyConstraints2D.None; //Unfreeze Y position to allow vertical movement after drag
            angle = Mathf.Atan2(rb.linearVelocityY, rb.linearVelocityX) * Mathf.Rad2Deg - 90f; // Calculate the angle of the projectile based on its velocity
            transform.rotation = Quaternion.Euler(0, 0, angle);  // Rotate the projectile to face the direction of movement
        }
    }
}
