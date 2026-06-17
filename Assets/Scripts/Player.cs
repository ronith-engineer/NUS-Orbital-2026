using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    [Header("Health")]
    [SerializeField] private Slider healthSlider;

    [Header("Movement")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private bool holdingWeapon;

    [Header("Stamina")]
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 20f;
    [SerializeField] private float staminaRegenRate = 10f;
    private float currentStamina;
    private bool isRunning;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private PlayerAimWeapon playerAimWeapon;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 1.4f;
    [SerializeField] private LayerMask whatIsGround;

    private float xInput;
    private bool isGrounded;
    private bool isCrouching;
    private int facingDirection = 1;

    protected override void Awake()
    {
        base.Awake();
        playerAimWeapon = GetComponent<PlayerAimWeapon>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    protected override void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        CheckGround();
        HandleCrouch();
        HandleStamina();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !holdingWeapon)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;
        base.Update();
    }

    private void HandleStamina()
    {
        bool wantsToRun = Input.GetKey(KeyCode.LeftControl)
                       && xInput != 0
                       && isGrounded
                       && !holdingWeapon
                       && currentStamina > 0;

        if (wantsToRun)
        {
            isRunning = true;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);
        }
        else
        {
            isRunning = false;
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
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

        float speed = isRunning ? runSpeed : moveSpeed;

        if (isCrouching)
            rb.linearVelocity = new Vector2(xInput * crouchSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    protected override void HandleFlip()
    {
        if (holdingWeapon)
        {
            if (playerAimWeapon.aimDirection.x < 0 && facingRight) Flip();
            else if (playerAimWeapon.aimDirection.x > 0 && !facingRight) Flip();
        }
        else
        {
            if (xInput > 0 && !facingRight) Flip();
            else if (xInput < 0 && facingRight) Flip();
        }
    }

    private void HandleCrouch()
    {
        isCrouching = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }

    protected override void HandleAnimations()
    {
        anim.SetFloat("xInput", xInput);
        anim.SetBool("isRunning", isRunning);
    }


    protected override void Die()
    {
        base.Die();
        Time.timeScale = 0f;
    }

}

