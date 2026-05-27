using UnityEngine;

public class Player : Entity
{
    [Header("Movement")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float crouchSpeed = 2.5f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 1.4f;
    [SerializeField] private LayerMask whatIsGround;

    private float xInput;
    private bool isGrounded;
    private bool isCrouching;
    private int facingDirection = 1;

    protected override void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        CheckGround();
        HandleCrouch();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x, jumpForce
            );

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();

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
        if (xInput != 0)
            facingDirection = xInput > 0 ? 1 : -1;

        if (isCrouching)
            rb.linearVelocity = new Vector2(
                xInput * crouchSpeed,
                rb.linearVelocity.y
            );
        else
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
        {
            //Debug.Log("Player flipping RIGHT");
            Flip();
        }
        else if (xInput < 0 && facingRight)
        {
            //Debug.Log("Player flipping LEFT");
            Flip();
        }
    }

    private void HandleCrouch()
    {
        isCrouching = Input.GetKey(KeyCode.S)
                   || Input.GetKey(KeyCode.DownArrow);
    }

    private void Shoot()
    {
        GameObject newBullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );
        newBullet.GetComponent<Bullet>()
                 .SetDirection(facingDirection);
    }

    protected override void HandleAnimations()
    {
        anim.SetFloat("xInput", xInput);
    }
}
