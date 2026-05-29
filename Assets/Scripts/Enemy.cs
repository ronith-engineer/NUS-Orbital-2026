using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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

    private bool isChasing;
    private bool wasChasingLastFrame;
    private bool playerDetectedForAttack;
    [SerializeField] private float moveAwayDistance = 1f;

    protected override void Update()
    {
        base.Update();
    }
    protected override void Awake()
    {
        base.Awake();
        targetPatrolPoint = patrolPointA.transform;
    }

    protected override void HandleCollision()
    {
        playerDetectedForAttack = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }
    
    private void HandleAttackAnimation()
    {
        anim.SetBool("attack", playerDetectedForAttack); 
    }

    protected override void HandleMovement()
    {
        if (Vector2.Distance(transform.position, playerPosition.position) < moveAwayDistance)
        {
            transform.position += moveAwayDistance * Vector3.right; 
        }
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
        anim.SetFloat("xVelocity",rb.linearVelocityX);
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
    }
}
