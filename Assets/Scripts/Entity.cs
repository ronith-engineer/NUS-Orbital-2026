using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
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
}