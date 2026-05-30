//using UnityEngine;

//public class Bullet : MonoBehaviour
//{
//    [SerializeField] private float bulletSpeed = 15f;
//    private int direction;

//    public void SetDirection(int dir)
//    {
//        direction = dir;
//    }

//    void Update()
//    {
//        transform.position += new Vector3(
//            bulletSpeed * direction * Time.deltaTime,
//            0,
//            0
//        );
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Enemy"))
//        {
//            collision.GetComponent<Entity>()?.TakeDamage();
//            Destroy(gameObject);
//        }

//        if (collision.CompareTag("Ground"))
//            Destroy(gameObject);
//    }

//    private void OnBecameInvisible()
//    {
//        Destroy(gameObject);
//    }
//}