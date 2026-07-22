using System;
using System.Collections;
using UnityEngine;

public class Shotgun : Weapon
{


    private float maxShootingDistance = 18f;


    public override void Initialize()
    {
        baseClipCapacity = 4f;
        baseAttackDamage = 8f;
        base.Initialize();
    }




    protected override IEnumerator Shoot()
    {

        currentAmmo--;
        if (NoiseManager.Instance != null)
            NoiseManager.Instance.SetShootNoise(shootNoise, 1f);
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, shootableLayers);

        if (hitInfo)
        {
            Debug.Log("Hit: " + hitInfo.transform.name);
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
                if (distance <= maxShootingDistance)
                {
                
                    float attackDamageWithDistance = currentAttackDamage * (1 - distance / maxShootingDistance);
                    enemy.TakeDamageFromEntity(player.facingRight, attackDamageWithDistance, knockbackForce); //damage decreases with distance, at max distance it will be 0, at point blank it will be full damage
                }
            }
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100f);
        }

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;


    }
}