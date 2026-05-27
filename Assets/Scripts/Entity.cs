using UnityEngine;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth;

    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 5f;
    protected bool facingRight = true;
    protected bool canMove = true;
    protected bool canJump = true;

    [Header("Attack Details")]
    [SerializeField] protected LayerMask whatIsTarget;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float attackRadius;

    [Header("Knockback Details")]
    [SerializeField] protected float knockbackForce = 5f;
    [SerializeField] protected float knockbackDuration = 0.5f;
    protected float knockbackTimer;
    [SerializeField] protected bool knockbackFromRight;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        if (canMove) HandleMovement();
        HandleAnimations();
        HandleFlip();
        HandleCollision();
        HandleDamage();
    }
    private void HandleDamage()
    {
        if (knockbackTimer > 0)
        {
            EnableMovementAndJump(false);
            if (knockbackFromRight)
            {
                rb.linearVelocity = new Vector2(-knockbackForce, knockbackForce);
            }
            else
            {
                rb.linearVelocity = new Vector2(knockbackForce, knockbackForce);
            }
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                EnableMovementAndJump(true);
            }
        }
        
    }

    protected virtual void HandleCollision() { }

    protected virtual void HandleMovement() { }

    protected virtual void HandleAnimations()
    {
        
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
        knockbackTimer = knockbackDuration;
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
        Time.timeScale = 0;
    }

    public void DamageTargets()
    {
        // Set knockback direction based on facing direction
        Collider2D[] targetColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);
        foreach (Collider2D target in targetColliders)
        {
            Entity entityTarget = target.GetComponent<Entity>();
            entityTarget.knockbackFromRight = !facingRight;

            entityTarget.TakeDamage();

        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }

    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }


}

