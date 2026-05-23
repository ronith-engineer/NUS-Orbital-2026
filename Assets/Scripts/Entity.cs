using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    

    [Header("Movement Details")]
    protected bool facingRight = true;

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



}