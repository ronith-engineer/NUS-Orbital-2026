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

    protected void HandleFlip()
    {
        if (rb.linearVelocity.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (rb.linearVelocity.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
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