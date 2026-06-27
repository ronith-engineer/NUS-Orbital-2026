using System.Data;
using TMPro;
using UnityEngine;

public class UpgradeInfo : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private WorkbenchPanelUI workbenchPanelUI;
    [SerializeField] private TextMeshProUGUI upgradeInfo;


    private void Update()
    {
        if (workbenchPanelUI.rowSelectPointer == 0)
        {
            upgradeInfo.text = "Damage - " + weaponManager.currentSelectedWeapon.countDamageUpgrades.ToString() + "/3"; 
        }
        else if (workbenchPanelUI.rowSelectPointer == 1)
        {
            upgradeInfo.text = "Clip Capacity - " + weaponManager.currentSelectedWeapon.countClipCapacityUpgrades.ToString() + "/3";
        }
}


}
