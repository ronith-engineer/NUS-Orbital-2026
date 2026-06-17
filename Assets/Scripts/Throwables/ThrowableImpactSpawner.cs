using System.Collections.Generic;
using UnityEngine;

public class ThrowableImpactSpawner : MonoBehaviour
{

    [SerializeField] private GameObject objectToSpawn; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall")) // Check if the collided object is tagged as "Ground" or "Wall"
        {

            // Loop through all contact points in the collision

            ContactPoint2D contact = collision.contacts[0];
            Debug.Log("Contact point: " + contact.point + ", Normal: " + contact.normal);
            Vector2 normal = contact.normal;
            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg - 90f; // Calculate the angle based on the normal vector and adjust it to point in the direction of the normal
            Quaternion spawnRotation = Quaternion.Euler(0f, 0f, angle);
            // Spawn the object at the collision point
            Instantiate(objectToSpawn, new Vector3(contact.point.x, contact.point.y, 0f), spawnRotation);


            Destroy(gameObject);// Destroy the throwable object after spawning the impact effect

        }

    }
}
