using UnityEngine;

public class Player : Entity
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 1.4f;
    [SerializeField] private LayerMask whatIsGround;

    private float xInput;
    private bool isGrounded;

    protected override void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x, jumpForce
            );

        base.Update();
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            groundCheckDistance,
            whatIsGround
        );
    }

    protected override void HandleMovement()
    {
        rb.linearVelocity = new Vector2(
            xInput * moveSpeed,
            rb.linearVelocity.y
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    protected override void HandleFlip()
    {
        if (xInput > 0 && !facingRight)
            Flip();
        else if (xInput < 0 && facingRight)
            Flip();
    }
}