using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected bool facingRight = true;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    protected virtual void HandleMovement() { }

    protected void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }

     protected void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    protected virtual void HandleFlip()
    {
        if (rb.linearVelocity.x > 0 && facingRight == false)
        {
            
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && facingRight == true)
        {
           
            Flip();
        }

        
    }
    public virtual void TakeDamage()
    {
        currentHealth--;
        Debug.Log(gameObject.name + " took damage! HP: " + currentHealth);
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
            Die();
    }
    private IEnumerator DamageFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject);
    }

}

