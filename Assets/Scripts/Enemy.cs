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
    [SerializeField] private float playerChaseRange;
    [SerializeField] private float lineOfSightRange;
    [SerializeField] private bool facingPlayer;
    private bool isChasing;

    protected override void Awake()
    {
        base.Awake();
        targetPatrolPoint = patrolPointA.transform;
        lineOfSightRange = playerChaseRange;
    }
    protected override void Update()
    {
        base.Update();
    }


    protected override void HandleMovement()
    {

        if (Vector2.Distance(transform.position, playerPosition.position) < lineOfSightRange)
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

            if (facingPlayer)
            {
                isChasing = true;
            }

        }


        if (isChasing)
        {
            if (playerPosition.position.x > transform.position.x)
            {
                rb.linearVelocityX = moveSpeed;
            }
            else
            {
                rb.linearVelocityX = -moveSpeed;
            }

            if (Vector2.Distance(transform.position, playerPosition.position) > playerChaseRange)
            {
                isChasing = false;
            }

        }
        else
        {
            if (Vector2.Distance(transform.position, playerPosition.position) < playerDetectionRangeNotFacing)
            {
                isChasing = true;
            }

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patrolPointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(patrolPointB.transform.position, 0.5f);
    }

    private IEnumerator EnemyWait()
    {
        //Debug.Log("Coroutine started");
        rb.linearVelocityX = 0;
        isWaiting = true;
        yield return new WaitForSeconds(3f);
        isWaiting = false;
        //Debug.Log("Coroutine ended");   
    }


}
