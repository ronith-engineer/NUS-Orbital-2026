using System;
using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Patrolling Details")]
    [SerializeField] private GameObject patrolPointA;
    [SerializeField] private GameObject patrolPointB;
    private Transform targetPatrolPoint;
    private bool isWaiting = false;

    [SerializeField] private Transform playerPosition;
    [SerializeField] private float playerDetectionRangeNotFacing;
    [SerializeField] private float lineOfSightRange;
    private bool facingPlayer;

    [Header("Enemy Type")]
    [SerializeField] private bool isBlind = false;

    private bool isChasing;
    private bool wasChasingLastFrame;
    private bool playerDetectedForAttack;
    [SerializeField] private float moveAwayDistance = 1f;
    [SerializeField] private float chaseSpeed;

    private CircleCollider2D lineOfSightCollider;
    private Transform eyeLevel;

    [Header("Noise Detection")]
    [SerializeField] private float investigateSpeed = 3f;
    private bool isInvestigatingNoise = false;
    private Vector2 lastHeardPosition;

    protected override void Update()
    {
        base.Update();
    }

    protected override void Awake()
    {
        base.Awake();
        targetPatrolPoint = patrolPointA.transform;
        lineOfSightCollider = GetComponentInChildren<CircleCollider2D>();
        eyeLevel = transform.Find("eyeLevelPosition");
    }

    protected override void HandleCollision()
    {
        if (isBlind && !isChasing)
        {
            playerDetectedForAttack = false;
            return;
        }
        playerDetectedForAttack = Physics2D.OverlapBox(attackPoint.position, attackBoxSize, 0f, whatIsTarget);

    }

    private void HandleAttackAnimation()
    {
        anim.SetBool("attack", playerDetectedForAttack);
        if (playerDetectedForAttack)
        {
            rb.linearVelocityX = 0f;
        }
    }

    public virtual void OnNoiseHeard(Vector2 noisePosition)
    {
        if (isChasing) return;
        if (isInvestigatingNoise) return;

        lastHeardPosition = noisePosition;
        isInvestigatingNoise = true;
    }

    protected override void HandleMovement()
    {
        if (isChasing)
        {
            if (playerPosition.position.x > transform.position.x)
                rb.linearVelocityX = chaseSpeed;
            else
                rb.linearVelocityX = -chaseSpeed;

            wasChasingLastFrame = true;
        }
        else if (isInvestigatingNoise)
        {
            float direction = lastHeardPosition.x - transform.position.x;

            if (Mathf.Abs(direction) > 0.5f)
            {
                rb.linearVelocityX = direction > 0 ? investigateSpeed : -investigateSpeed;
            }
            else
            {
                isInvestigatingNoise = false;
                StartCoroutine(EnemyWait());
            }
        }
        else
        {
            if (wasChasingLastFrame)
            {
                if (isWaiting) return;
                StartCoroutine(EnemyWait());
            }
            else
            {
                if (isWaiting) return;
                if (Mathf.Abs(transform.position.x - targetPatrolPoint.position.x) >= 0.5f)
                    ReturnToPatrolPoint();
                else
                    HandlePatrolling();
            }
        }
    }

    private void ReturnToPatrolPoint()
    {
        if (targetPatrolPoint.position.x > transform.position.x)
            rb.linearVelocityX = moveSpeed;
        else if (targetPatrolPoint.position.x < transform.position.x)
            rb.linearVelocityX = -moveSpeed;
    }

    private void HandlePatrolling()
    {
        if (isWaiting) return;
        if (targetPatrolPoint == patrolPointA.transform)
            rb.linearVelocityX = -moveSpeed;
        else
            rb.linearVelocityX = moveSpeed;

        if (Mathf.Abs(transform.position.x - targetPatrolPoint.position.x) < 0.5f && targetPatrolPoint == patrolPointA.transform)
        {
            StartCoroutine(EnemyWait());
            targetPatrolPoint = patrolPointB.transform;
        }
        else if (Mathf.Abs(transform.position.x - targetPatrolPoint.position.x) < 0.5f && targetPatrolPoint == patrolPointB.transform)
        {
            StartCoroutine(EnemyWait());
            targetPatrolPoint = patrolPointA.transform;
        }
    }

    private void checkFacingPlayer()
    {
        if (playerPosition.position.x > transform.position.x && facingRight)
            facingPlayer = true;
        else if (playerPosition.position.x < transform.position.x && !facingRight)
            facingPlayer = true;
        else
            facingPlayer = false;
    }

    private IEnumerator EnemyWait()
    {
        rb.linearVelocityX = 0;
        isWaiting = true;
        yield return new WaitForSeconds(3f);
        isWaiting = false;
        wasChasingLastFrame = false;
    }

    protected override void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocityX);
        HandleAttackAnimation();
        anim.SetBool("isChasing", isChasing);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(patrolPointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(patrolPointB.transform.position, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lineOfSightRange);
        Gizmos.matrix = Matrix4x4.TRS(attackPoint.position, attackPoint.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackBoxSize);
        Gizmos.matrix = Matrix4x4.identity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (isBlind) return;

            Player player = collision.GetComponent<Player>();
            if (player != null && player.IsInShadow())
                return;

            RaycastHit2D colliderInSight = Physics2D.Raycast(eyeLevel.position, eyeLevel.transform.right, lineOfSightRange, whatIsTarget);

            if (colliderInSight)
            {
                Player detectedPlayer = colliderInSight.transform.GetComponent<Player>();
                if (detectedPlayer != null)
                {
                    isChasing = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isChasing)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null && player.IsInShadow())
                return;


            if (isInvestigatingNoise)
            {
                isChasing = true;
                isInvestigatingNoise = false;
                return;
            }

            if (isBlind) return;


            RaycastHit2D colliderInSight = Physics2D.Raycast(eyeLevel.position, eyeLevel.transform.right, lineOfSightRange, whatIsTarget);
            if (colliderInSight)
            {
                Player detectedPlayer = colliderInSight.transform.GetComponent<Player>();
                if (detectedPlayer != null)
                {
                    isChasing = true;
                }
            }
        }
    }
}