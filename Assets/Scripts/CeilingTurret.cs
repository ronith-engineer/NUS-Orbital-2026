using System.Collections;
using UnityEngine;

public class CeilingTurret : MonoBehaviour
{
    [Header("Sweep")]
    [SerializeField] private float sweepAngle = 45f;
    [SerializeField] private float sweepSpeed = 30f;

    [Header("Laser")]
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private float laserRange = 20f;
    [SerializeField] private LayerMask whatToHit;

    [Header("Shooting")]
    [SerializeField] private LineRenderer shotLine;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float knockbackForce = 2f;
    private float fireTimer;

    [Header("Fire Point")]
    [SerializeField] private Transform firePoint;

    [Header("Turret Health")]
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private float disableTime = 3f;
    private float currentHealth;
    private bool isDisabled = false;

    [Header("Damage Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float currentAngle = 0f;
    private bool sweepingRight = true;
    private bool playerDetected = false;

    private void Start()
    {
        shotLine.enabled = false;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDisabled) return;

        DetectAndShoot();

        if (!playerDetected)
            Sweep();
    }

    private void Sweep()
    {
        if (sweepingRight)
        {
            currentAngle += sweepSpeed * Time.deltaTime;
            if (currentAngle >= sweepAngle)
                sweepingRight = false;
        }
        else
        {
            currentAngle -= sweepSpeed * Time.deltaTime;
            if (currentAngle <= 0f)
                sweepingRight = true;
        }

        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }

    private void DetectAndShoot()
    {
        Vector2 origin = firePoint != null ? firePoint.position : transform.position;
        Vector2 direction = -transform.up;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, laserRange, whatToHit);

        laserLine.SetPosition(0, origin);

        if (hit)
        {
            laserLine.SetPosition(1, hit.point);

            Player player = hit.transform.GetComponent<Player>();
            Enemy enemy = hit.transform.GetComponent<Enemy>();

            if (player != null || enemy != null)
            {
                playerDetected = true;
                bool turretIsLeft = transform.position.x < hit.transform.position.x;

                fireTimer -= Time.deltaTime;
                if (fireTimer <= 0f)
                {
                    StartCoroutine(ShootLine(origin, hit.point));

                    if (player != null)
                        player.TakeDamageFromEntity(turretIsLeft, damage, knockbackForce);

                    if (enemy != null)
                        enemy.TakeDamageFromEntity(turretIsLeft, damage, knockbackForce);

                    fireTimer = fireRate;
                }
            }
            else
            {
                playerDetected = false;
            }
        }
        else
        {
            playerDetected = false;
            laserLine.SetPosition(1, origin + direction * laserRange);
            fireTimer = 0f;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDisabled) return;

        currentHealth -= damageAmount;
        StartCoroutine(DamageBlink());

        if (currentHealth <= 0f)
        {
            StartCoroutine(DisableTurret());
        }
    }

    private IEnumerator DamageBlink()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    private IEnumerator DisableTurret()
    {
        isDisabled = true;
        laserLine.enabled = false;
        shotLine.enabled = false;

        yield return new WaitForSeconds(disableTime);

        currentHealth = maxHealth;
        isDisabled = false;
        laserLine.enabled = true;
    }

    private IEnumerator ShootLine(Vector2 start, Vector2 hitPoint)
    {
        shotLine.SetPosition(0, start);
        shotLine.SetPosition(1, hitPoint);
        shotLine.enabled = true;
        yield return new WaitForSeconds(0.02f);
        shotLine.enabled = false;
    }
}