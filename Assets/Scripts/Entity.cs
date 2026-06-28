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
    [SerializeField] protected int maxHealth = 10;
    [SerializeField] protected float currentHealth;

    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 5f;
    public bool facingRight = true;
    [SerializeField] protected bool canMove = true;
    [SerializeField] protected bool canJump = true;

    [Header("Attack Details")]
    [SerializeField] protected LayerMask whatIsTarget;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Vector2 attackBoxSize;
    [SerializeField] private int attackDamage = 1;

    [Header("Knockback Details")]
    [SerializeField] protected float receivedKnockbackForce;
    [SerializeField] protected float knockbackDuration = 0.5f;
    [SerializeField] private float knockbackForce = 6f;
    protected float knockbackTimer;
    protected bool knockbackFromRight;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        if (currentHealth <= 0) Die();
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
                rb.linearVelocity = new Vector2(-receivedKnockbackForce, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(receivedKnockbackForce, 0);
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

     public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    protected virtual void HandleFlip()
    {
        if (knockbackTimer > 0) return;
        if (rb.linearVelocity.x > 0 && facingRight == false)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && facingRight == true)
        {
            Flip();
        }

        
    }

    public void Heal()
    {
        if (currentHealth <= 5)
            currentHealth += 5;
        else
            currentHealth = maxHealth;
    }

    public virtual void TakeDamageFromEntity(bool attackerFacingRight, float attackDamage, float knockbackForce)
    {

        currentHealth -= attackDamage;
        // Determine knockback direction based on attacker position
        knockbackFromRight = !attackerFacingRight;
        receivedKnockbackForce = knockbackForce;
        knockbackTimer = knockbackDuration;
        Debug.Log(gameObject.name + " took damage! HP: " + currentHealth);
        StartCoroutine(DamageFlash());


    }

    public virtual void TakeDamageFromHazard(float attackDamage)
    {
        currentHealth -= attackDamage;
        Debug.Log(gameObject.name + " took damage from hazard! HP: " + currentHealth);
        StartCoroutine(DamageFlash());

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

    public void DamageTargets()
    {
        // Set knockback direction based on facing direction
        Collider2D[] targetColliders = Physics2D.OverlapBoxAll (attackPoint.position, attackBoxSize, 0f, whatIsTarget);

        foreach (Collider2D target in targetColliders)
        {
            Entity entityTarget = target.GetComponent<Entity>();
            entityTarget.TakeDamageFromEntity(facingRight, attackDamage, knockbackForce);

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

