using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;

    [SerializeField] protected Transform firePoint;
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected LayerMask excludeLayers;
    protected int shootableLayers;

    protected Player player;
    private Coroutine shootCoroutine;
    public float currentAmmo;
    public float reserveAmmo;
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

    protected Animator anim;

    [SerializeField] protected bool canReload = true;
    [SerializeField] protected bool canShoot = true;

    [SerializeField] protected float shootNoise = 90f;
    [SerializeField] protected float shootNoiseRadius = 15f;


    public virtual void Initialize()
    {
        currentAttackDamage = baseAttackDamage;
        currentClipCapacity = baseClipCapacity;
        currentAmmo = currentClipCapacity;
    }

    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();
        shootableLayers = ~excludeLayers;
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0 && canShoot)
        {
            anim.SetTrigger("shoot");
            shootCoroutine = StartCoroutine(Shoot());
        }

        if (Input.GetKeyDown(KeyCode.R) && reserveAmmo > 0 && canReload)
        {
            anim.SetTrigger("reload");
            ReloadWeapon();
        }
    }

    protected virtual IEnumerator Shoot()
    {
        currentAmmo--;
        if (NoiseManager.Instance != null)
        {
            NoiseManager.Instance.SetShootNoise(shootNoise, 1f);
            NoiseManager.Instance.MakeNoise(firePoint.position, shootNoiseRadius);
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, shootableLayers);

        if (hitInfo)
        {
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
        lineRenderer.enabled = false;
    }

    private void GetCurrentStats()
    {
        currentAttackDamage = baseAttackDamage;
        currentClipCapacity = baseClipCapacity;
        Debug.Log(gameObject.name + " GetCurrentStats: appliedUpgrades.Count = " + appliedUpgrades.Count + ", baseAttackDamage = " + baseAttackDamage);

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

    public void AddToReserveAmmo(float ammoIncrease)
    {
        reserveAmmo += ammoIncrease;
    }

    private void ReloadWeapon()
    {
        float ammoNeeded = currentClipCapacity - currentAmmo;
        if (reserveAmmo > ammoNeeded)
        {
            currentAmmo += ammoNeeded;
            reserveAmmo -= ammoNeeded;
        }
        else
        {
            currentAmmo += reserveAmmo;
            reserveAmmo -= reserveAmmo;
        }
    }

    public void EnableReloadAndShoot(bool canReloadAndShoot)
    {
        canReload = canReloadAndShoot;
        canShoot = canReloadAndShoot;
    }


    public float CountUpgrades(WeaponUpgrade weaponUpgrade)
    {
        if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.Damage)
        {
            return countDamageUpgrades;
        }
        else if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.ClipCapacity)
        {
            return countClipCapacityUpgrades;
        }
        else
        {
            return 0;
        }

    }

}

