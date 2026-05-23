using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : Entity
{
    [SerializeField] private GameObject patrolPointA;
    [SerializeField] private GameObject patrolPointB;
    private Transform currentPoint;
    private bool isWaiting = false;
    protected override void Awake()
    {
        base.Awake();
        currentPoint = patrolPointA.transform;
    }
    protected override void Update()
    {
        base.Update();
        PlayerDetection();
    }


    protected override void HandleMovement()
    {
        if (isWaiting) return;
        if (currentPoint == patrolPointA.transform)
        {
            rb.linearVelocityX = -moveSpeed;
        }
        else
        {
            rb.linearVelocityX = moveSpeed;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == patrolPointA.transform)
        {
            StartCoroutine(MyCoroutine());
            currentPoint = patrolPointB.transform;
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == patrolPointB.transform)
        {
            StartCoroutine(MyCoroutine());
            currentPoint = patrolPointA.transform;
        }
    }

    private void PlayerDetection()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patrolPointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(patrolPointB.transform.position, 0.5f);
    }

    private IEnumerator MyCoroutine()
    {
        //Debug.Log("Coroutine started");
        rb.linearVelocityX = 0;
        isWaiting = true;
        yield return new WaitForSeconds(3f);
        isWaiting = false;
        //Debug.Log("Coroutine ended");   
    }


}
