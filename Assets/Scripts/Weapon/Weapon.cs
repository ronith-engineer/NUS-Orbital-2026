using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{

    public string weaponName;

    [SerializeField] protected Transform firePoint;
    [SerializeField] protected LineRenderer lineRenderer;
    
    protected Player player;
    private Coroutine shootCoroutine;
    public float currentAmmo;
    protected float baseClipCapacity;
    private float currentClipCapacity;
    protected float baseAttackDamage;
    protected float currentAttackDamage;
    [SerializeField] protected float knockbackForce;

    public List<WeaponUpgrade> appliedUpgrades;
    public int countDamageUpgrades = 0;
    public int countClipCapacityUpgrades = 0;

    [SerializeField] public Sprite icon;
    [SerializeField] public Sprite weaponImage;



    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();
        currentAttackDamage = baseAttackDamage;
        currentClipCapacity = baseClipCapacity;
        currentAmmo = currentClipCapacity;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0)
        {
            shootCoroutine = StartCoroutine(Shoot());
            Debug.Log("Current Damage Output " + currentAttackDamage + " Current Clip Capacity: " + currentClipCapacity);
        }
    }


    

    protected virtual IEnumerator Shoot()
    {
        currentAmmo--;
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        Debug.Log("firePoint.right: " + firePoint.right);

        if (hitInfo)
        {
            Debug.Log("Hit: " + hitInfo.transform.name);
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamageFromEntity(player.facingRight, currentAttackDamage, knockbackForce);
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
        //wait for a short time and then disable the line renderer

        lineRenderer.enabled = false;

    }

    private void GetCurrentStats()
    {
        currentAttackDamage = baseAttackDamage;
        currentClipCapacity = baseClipCapacity;
        foreach (WeaponUpgrade upgrade in appliedUpgrades)
        {
            if (upgrade.statType == WeaponUpgrade.UpgradeStatType.Damage)
            {
                currentAttackDamage += upgrade.statIncrease;
            }
            else if (upgrade.statType == WeaponUpgrade.UpgradeStatType.ClipCapacity)
            {
                currentClipCapacity += upgrade.statIncrease;
            }

        }
    }


    public void ApplyUpgrade(WeaponUpgrade upgrade)
    {
        if (upgrade.statType == WeaponUpgrade.UpgradeStatType.Damage)
        {
            if (countDamageUpgrades >= 3) return;
            appliedUpgrades.Add(upgrade);
            countDamageUpgrades++;
        }
        else if (upgrade.statType == WeaponUpgrade.UpgradeStatType.ClipCapacity)
        {
            if (countClipCapacityUpgrades >= 3) return;
            appliedUpgrades.Add(upgrade);
            countClipCapacityUpgrades++;
        }

        GetCurrentStats();
    }
}
