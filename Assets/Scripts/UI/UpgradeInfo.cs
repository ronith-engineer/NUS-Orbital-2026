//using System.Data;
//using TMPro;
//using UnityEngine;

//public class UpgradeInfo : MonoBehaviour
//{
//    [SerializeField] private WeaponManager weaponManager;

//    [SerializeField] private TextMeshProUGUI upgradeInfo;



//    private void OnEnable()
//    {
//       WorkbenchPanelUI.Instance.upgradeRow1.OnUpgradeRowSelected += UpdateUpgradeInfo;
//       WorkbenchPanelUI.Instance.upgradeRow2.OnUpgradeRowSelected += UpdateUpgradeInfo;
//       WorkbenchPanelUI.Instance.upgradeRow1.OnUpgradeApplied += UpdateUpgradeInfo;
//       WorkbenchPanelUI.Instance.upgradeRow2.OnUpgradeApplied += UpdateUpgradeInfo;

//    }

//    private void OnDisable()
//    {
//        WorkbenchPanelUI.Instance.upgradeRow1.OnUpgradeRowSelected -= UpdateUpgradeInfo;
//        WorkbenchPanelUI.Instance.upgradeRow2.OnUpgradeRowSelected -= UpdateUpgradeInfo;
//        WorkbenchPanelUI.Instance.upgradeRow1.OnUpgradeApplied -= UpdateUpgradeInfo;
//        WorkbenchPanelUI.Instance.upgradeRow2.OnUpgradeApplied -= UpdateUpgradeInfo;
//    }

//    //private void Update()
//    //{
//    //    if (weaponManager.currentSelectedWeapon == null) return;
//    //    if (workbenchPanelUI.rowSelectPointer == 0)
//    //    {
//    //        upgradeInfo.text = "Damage - " + weaponManager.currentSelectedWeapon.countDamageUpgrades.ToString() + "/3";


//    //    }
//    //    else if (workbenchPanelUI.rowSelectPointer == 1)
//    //    {
//    //        upgradeInfo.text = "Clip Capacity - " + weaponManager.currentSelectedWeapon.countClipCapacityUpgrades.ToString() + "/3";
//    //    }

//    //}

//    private void UpdateUpgradeInfo(WeaponUpgrade weaponUpgrade)
//    {
//        if (weaponManager.currentSelectedWeapon == null) return;
//        upgradeInfo.text = weaponUpgrade.statType.ToString() + " - " + weaponManager.currentSelectedWeapon.CountUpgrades(weaponUpgrade).ToString() + "/3\n\n" + "Upgrade Cost : " + weaponUpgrade.partsCost.ToString() + "\n" + "Parts in Hand : " + PartsManager.Instance.currentParts.ToString();
//    }
//}

using System.Data;
using TMPro;
using UnityEngine;

public class UpgradeInfo : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private TextMeshProUGUI upgradeInfo;
    private void OnEnable()
    {
        Debug.Log("[UpgradeInfo] OnEnable called");
        WorkbenchPanelUI.Instance.upgradeRow1.OnUpgradeRowSelected += UpdateUpgradeInfo;
        WorkbenchPanelUI.Instance.upgradeRow2.OnUpgradeRowSelected += UpdateUpgradeInfo;
        WorkbenchPanelUI.Instance.upgradeRow1.OnUpgradeApplied += UpdateUpgradeInfo;
        WorkbenchPanelUI.Instance.upgradeRow2.OnUpgradeApplied += UpdateUpgradeInfo;

        Debug.Log("[UpgradeInfo] row1.CurrentUpgrade = " + WorkbenchPanelUI.Instance.upgradeRow1.CurrentUpgrade);
        if (WorkbenchPanelUI.Instance.upgradeRow1.CurrentUpgrade != null)
            UpdateUpgradeInfo(WorkbenchPanelUI.Instance.upgradeRow1.CurrentUpgrade);
    }
    private void OnDisable()
    {
        WorkbenchPanelUI.Instance.upgradeRow1.OnUpgradeRowSelected -= UpdateUpgradeInfo;
        WorkbenchPanelUI.Instance.upgradeRow2.OnUpgradeRowSelected -= UpdateUpgradeInfo;
        WorkbenchPanelUI.Instance.upgradeRow1.OnUpgradeApplied -= UpdateUpgradeInfo;
        WorkbenchPanelUI.Instance.upgradeRow2.OnUpgradeApplied -= UpdateUpgradeInfo;
    }

    private void UpdateUpgradeInfo(WeaponUpgrade weaponUpgrade)
    {
        if (weaponUpgrade == null) return;

        if (weaponUpgrade.statType == WeaponUpgrade.UpgradeStatType.ClipCapacity)
        {
            upgradeInfo.text = "Clip Capacity" + " - " +
                                WorkbenchPanelUI.Instance.CurrentWeapon.CountUpgrades(weaponUpgrade).ToString() + "/3\n\n" +
                                "Upgrade Cost : " + weaponUpgrade.partsCost.ToString() + "\n" +
                                "Parts in Hand : " + PartsManager.Instance.currentParts.ToString();

        }
        else
        {
            upgradeInfo.text = weaponUpgrade.statType.ToString() + " - " +
                                WorkbenchPanelUI.Instance.CurrentWeapon.CountUpgrades(weaponUpgrade).ToString() + "/3\n\n" +
                                "Upgrade Cost : " + weaponUpgrade.partsCost.ToString() + "\n" +
                                "Parts in Hand : " + PartsManager.Instance.currentParts.ToString();
        }
    }
}

