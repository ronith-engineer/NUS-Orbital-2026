using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeRowUI : MonoBehaviour
{
    [SerializeField] private List<Image> fillBoxes;
    [SerializeField] private TextMeshProUGUI statLabel;
    [SerializeField] public GameObject selectedHighlight;

    public event Action<WeaponUpgrade> OnUpgradeRowSelected;
    public event Action<WeaponUpgrade> OnUpgradeApplied;
    public WeaponUpgrade CurrentUpgrade => weaponUpgrade;

    private Weapon weaponRef;
    private WeaponUpgrade weaponUpgrade;
    private float targetHoldDuration = 2f;
    private float holdTimer;
    private bool isFocused;
    private bool isApplying;
    private bool maxed;

    private void Update()
    {
        if (weaponRef == null || weaponUpgrade == null) return;
        if (!isFocused) return;

        maxed = IsMaxed();
        if (maxed) return; 

        int currentCount = GetCurrentCount();

        if (Input.GetKey(KeyCode.Space) && !isApplying && CanApplyUpgrade())
        {
            holdTimer += Time.deltaTime;
            fillBoxes[currentCount].fillAmount = Mathf.Clamp01(holdTimer / targetHoldDuration);

            if (holdTimer >= targetHoldDuration)
            {
                isApplying = true;
                ApplyUpgrade();
            }
        }
        else if (!Input.GetKey(KeyCode.Space))
        {
            holdTimer = 0;
            isApplying = false;

            if (currentCount < fillBoxes.Count)
                fillBoxes[currentCount].fillAmount = 0;
        }
    }

    public void Setup(Weapon weapon, WeaponUpgrade upgrade)
    {
        selectedHighlight.SetActive(false);
        weaponRef = weapon;
        weaponUpgrade = upgrade;
        statLabel.text = upgrade.statType.ToString();
        RefreshBoxes();
    }

    private void RefreshBoxes()
    {
        int currentCount = GetCurrentCount();
        for (int i = 0; i < fillBoxes.Count; i++)
        {
            fillBoxes[i].fillAmount = i < currentCount ? 1f : 0f;
        }
    }

    private int GetCurrentCount()
    {
        if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.Damage)
            return weaponRef.countDamageUpgrades;
        else if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.ClipCapacity)
            return weaponRef.countClipCapacityUpgrades;
        return 0;
    }

    private void ApplyUpgrade()
    {
        if (PartsManager.Instance.SpendParts(weaponUpgrade.partsCost))
        {
            weaponRef.ApplyUpgrade(weaponUpgrade);
            RefreshBoxes(); // snaps just-completed box to 1, keeps prior boxes at 1
            OnUpgradeApplied?.Invoke(weaponUpgrade);
        }
        holdTimer = 0;
    }

    public void SetFocused(bool focused)
    {
        isFocused = focused;
        selectedHighlight.SetActive(focused);
        if (focused) OnUpgradeRowSelected?.Invoke(weaponUpgrade);

        if (!focused)
        {
            holdTimer = 0;
            isApplying = false;
            if (!IsMaxed())
            {
                int currentCount = GetCurrentCount();
                if (currentCount < fillBoxes.Count)
                    fillBoxes[currentCount].fillAmount = 0;
            }
        }
    }

    private bool IsMaxed()
    {
        if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.Damage)
            return weaponRef.countDamageUpgrades >= 3;
        else if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.ClipCapacity)
            return weaponRef.countClipCapacityUpgrades >= 3;
        return false;
    }

    private bool CanApplyUpgrade()
    {
        return PartsManager.Instance.CanAffordUpgrade(weaponUpgrade.partsCost);
    }
}