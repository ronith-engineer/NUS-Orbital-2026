using UnityEngine;

public class Player : Entity
{
    [SerializeField] private float moveSpeed = 5f;
    private float xInput;

    protected override void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        base.Update();
    }

    protected override void HandleMovement()
    {
        rb.linearVelocity = new Vector2(
            xInput * moveSpeed,
            rb.linearVelocity.y
        );
    }
}