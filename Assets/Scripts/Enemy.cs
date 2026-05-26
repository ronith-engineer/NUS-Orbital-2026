using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : Entity
{
    [Header("Patrol Points")]
    [SerializeField] private GameObject patrolPointA;
    [SerializeField] private GameObject patrolPointB;

    private Transform targetPatrolPoint;
    private bool isWaiting = false;

    [SerializeField] private Transform playerPosition;
    [SerializeField] private float playerDetectionRangeNotFacing;
    [SerializeField] private float lineOfSightRange;
    [SerializeField] private bool facingPlayer;

    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;

    private bool isChasing;
    private bool wasChasingLastFrame;

    protected override void Awake()
    {
        base.Awake();
        targetPatrolPoint = patrolPointA.transform;
    }
    protected override void Update()
    {
        if (canMove) HandleMovement();
        HandleAnimations();
        HandleFlip();
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Vector2.Distance(transform.position, playerPosition.position) < 1.5f)
        {
            canMove = false;
            anim.SetBool("attack", true);
        }
        else
        {
            canMove = true;
            anim.SetBool("attack", false);
        }
    }
    

    protected override void HandleMovement()
    {

        CheckIsChasing(); // Check if the enemy should be chasing the player or patrolling
        if (isChasing)
        {
            if (playerPosition.position.x > transform.position.x)
            {
                rb.linearVelocityX = moveSpeed*2;
            }
            else
            {
                rb.linearVelocityX = -moveSpeed*2;
            }
            wasChasingLastFrame = true;
        }
        else
        {   
            if (wasChasingLastFrame)
            {
                if (isWaiting) return; // If already waiting, do nothing
                StartCoroutine(EnemyWait()); // Start the waiting coroutine to create a pause before resuming patrol      
            }
            else
            {
                if (isWaiting) return; // If already waiting, do nothing
                if (Vector2.Distance(transform.position, targetPatrolPoint.position) >= 0.5f)
                {
                    ReturnToPatrolPoint();
                }
                else
                {
                    HandlePatrolling();
                }
                
            }

        }
    }

    private void ReturnToPatrolPoint()
    {
        if (targetPatrolPoint.position.x > transform.position.x)
        {
            rb.linearVelocityX = moveSpeed;
        }
        else if (targetPatrolPoint.position.x < transform.position.x)
        {
            rb.linearVelocityX = -moveSpeed;
        }
    }

    private void HandlePatrolling()
    {

        if (isWaiting) return;
        if (targetPatrolPoint == patrolPointA.transform)
        {
            rb.linearVelocityX = -moveSpeed;
        }
        else
        {
            rb.linearVelocityX = moveSpeed;
        }

        if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 0.5f && targetPatrolPoint == patrolPointA.transform)
        {
            StartCoroutine(EnemyWait());
            targetPatrolPoint = patrolPointB.transform;
        }
        else if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 0.5f && targetPatrolPoint == patrolPointB.transform)
        {
            StartCoroutine(EnemyWait());
            targetPatrolPoint = patrolPointA.transform;
        }
    }

    private void CheckIsChasing()
    {
        if (Vector2.Distance(transform.position, playerPosition.position) < lineOfSightRange)
        {
            checkFacingPlayer();
      
            if (facingPlayer)
            {
                isChasing = true;
            }
            else
            {
                if (Vector2.Distance(transform.position, playerPosition.position) < playerDetectionRangeNotFacing)
                {
                    isChasing = true;
                }
                else
                {
                    isChasing = false;
                }
            }
        }
        else 
        {
            isChasing = false;
        }

    }

    private void checkFacingPlayer()
    {
        if (playerPosition.position.x > transform.position.x && facingRight)
        {
            facingPlayer = true;
        }
        else if (playerPosition.position.x < transform.position.x && !facingRight)
        {
            facingPlayer = true;
        }
        else
        {
            facingPlayer = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(patrolPointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(patrolPointB.transform.position, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lineOfSightRange);
    }

    private IEnumerator EnemyWait()
    {
        //Debug.Log("Coroutine started");
        rb.linearVelocityX = 0;
        isWaiting = true;
        yield return new WaitForSeconds(3f);
        isWaiting = false;
        wasChasingLastFrame = false;
        //Debug.Log("Coroutine ended");   
    }

    protected override void HandleAnimations()
    {
        base.HandleAnimations();
        anim.SetBool("isChasing", isChasing);
    }

    public void DamageTargets()
    {
        Collider2D[] targetColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);
        foreach (Collider2D target in targetColliders)
        {
            Entity entityTarget = target.GetComponent<Entity>();
            entityTarget.TakeDamage();

        }
    }

}
