using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeRowUI : MonoBehaviour
{
    [SerializeField] private Slider sliderFill;
    [SerializeField] private TextMeshProUGUI statLabel;
    [SerializeField] public GameObject selectedHighlight;

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

        if (maxed)
        {
            sliderFill.value = 1f; 
            return;
        }

        if (Input.GetKey(KeyCode.Space) && !isApplying)
        {
            holdTimer += Time.deltaTime;
            sliderFill.value = Mathf.Lerp(0, 1, holdTimer / targetHoldDuration);

            if (holdTimer >= targetHoldDuration)
            {
                isApplying = true;
                ApplyUpgrade();
                Debug.Log("Damage Upgrades Count: " + weaponRef.countDamageUpgrades + " Clip Capacity upgrades count: " + weaponRef.countClipCapacityUpgrades);
            }
        }
        else if (!Input.GetKey(KeyCode.Space))
        {
            holdTimer = 0;
            sliderFill.value = 0;
            isApplying = false;
        }
    }

    public void Setup(Weapon weapon, WeaponUpgrade upgrade)
    {
        selectedHighlight.SetActive(false);
        weaponRef = weapon;
        weaponUpgrade = upgrade;
        statLabel.text = upgrade.statType.ToString();
        RefreshSlider();
    }

    private void RefreshSlider()
    {
        int currentCount = 0;

        if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.Damage)
            currentCount = weaponRef.countDamageUpgrades;
        else if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.ClipCapacity)
            currentCount = weaponRef.countClipCapacityUpgrades;

        sliderFill.value = currentCount >= 3 ? 1f : 0f;
    }

    private void ApplyUpgrade()
    {
        weaponRef.ApplyUpgrade(weaponUpgrade);
        holdTimer = 0;
        sliderFill.value = 0; 
    }

    public void SetFocused(bool focused)
    {
        isFocused = focused;
        selectedHighlight.SetActive(focused);
        
        if (!focused) 
        {
            holdTimer = 0;
            isApplying = false;
            if (!IsMaxed()) sliderFill.value = 0;

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


}