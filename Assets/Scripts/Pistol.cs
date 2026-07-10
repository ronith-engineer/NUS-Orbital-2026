using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{




    protected override void Awake()
    {
        baseClipCapacity = 4;
        baseAttackDamage = 2;

        base.Awake();

    }



    //protected override IEnumerator Shoot()
    //{
    //    currentAmmo--;
    //    if (NoiseManager.Instance != null)
    //        NoiseManager.Instance.SetShootNoise(shootNoise, 1f);
    //    RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, shootableLayers);


    //    if (hitInfo)
    //    {
    //        Debug.Log("Hit: " + hitInfo.transform.name);
    //        Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
    //        if (enemy != null)
    //        {
    //            enemy.TakeDamageFromEntity(player.facingRight, currentAttackDamage, knockbackForce);
    //        }
    //        lineRenderer.SetPosition(0, firePoint.position);
    //        lineRenderer.SetPosition(1, hitInfo.point);
    //    }
    //    else
    //    {
    //        lineRenderer.SetPosition(0, firePoint.position);
    //        lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100f);
    //    }

    //    lineRenderer.enabled = true;


    //    yield return new WaitForSeconds(0.02f);

    //    lineRenderer.enabled = false;

    //}

}
