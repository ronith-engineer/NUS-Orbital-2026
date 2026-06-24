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
    private float targetHoldDuration = 3f;
    private float holdTimer;
    private bool isFocused;


    private void Update()
    {
        if (!isFocused) return;

        if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.Damage)
        {
            if (weaponRef.countDamageUpgrades > 3) return;
        }
        else if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.ClipCapacity)
        {
            if (weaponRef.countClipCapacityUpgrades > 3) return;

        }

        if (Input.GetKey(KeyCode.Space))
        {
            holdTimer += Time.deltaTime;
            sliderFill.value = Mathf.Lerp(0,1,holdTimer/targetHoldDuration);
            if (holdTimer >= targetHoldDuration)
            {
                ApplyUpgrade();
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            holdTimer = 0;
            sliderFill.value = 0;

        }
    }




    public void Setup(Weapon weapon, WeaponUpgrade upgrade)
    {
        selectedHighlight.SetActive(false);
        weaponRef = weapon;
        weaponUpgrade = upgrade;
        statLabel.text = upgrade.statType.ToString();

    }

    private void ApplyUpgrade()
    {
        weaponRef.ApplyUpgrade(weaponUpgrade);
        holdTimer = 0;

    }

    public void SetFocused(bool focused)
    {
        isFocused = focused;
        selectedHighlight.SetActive(focused);
    }

   
}
