using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkbenchPanelUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private Image weaponImage;
    [SerializeField] private WeaponManager weaponManager;

    [Header("Upgrade Rows")]
    [SerializeField] private UpgradeRowUI upgradeRow1;
    [SerializeField] private UpgradeRowUI upgradeRow2;

    [Header("Weapon Upgrades")]
    [SerializeField] private WeaponUpgrade weaponUpgrade1;
    [SerializeField] private WeaponUpgrade weaponUpgrade2;

    public int rowSelectPointer;
    private int countUpgradeRows;


    private List<UpgradeRowUI> upgradeRows => new List<UpgradeRowUI>() { upgradeRow1, upgradeRow2};

    private void Start()
    {
        countUpgradeRows = upgradeRows.Count;
        Open();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rowSelectPointer += 1;
            rowSelectPointer = Mathf.Clamp(rowSelectPointer, 0, countUpgradeRows - 1);
            Debug.Log(rowSelectPointer);
            SelectUpgradeRow(rowSelectPointer);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rowSelectPointer -= 1;
            rowSelectPointer = Mathf.Clamp(rowSelectPointer, 0, countUpgradeRows - 1);
            Debug.Log(rowSelectPointer);
            SelectUpgradeRow(rowSelectPointer);
        }
    }



   

    public void Open()
    {
        Weapon currentSelectedWeapon = weaponManager.currentSelectedWeapon;
        weaponNameText.text = currentSelectedWeapon.weaponName;
        SetWeaponImage(currentSelectedWeapon);
        upgradeRow1.Setup(currentSelectedWeapon, weaponUpgrade1);
        upgradeRow2.Setup(currentSelectedWeapon, weaponUpgrade2);
        SelectUpgradeRow(0);

    }

    private void OnEnable()
    {
        weaponManager.OnSelectedWeaponChanged += Open;
        Debug.Log("Open called");
        EventSystem.current.sendNavigationEvents = false;

    }

    private void OnDisable()
    {
        weaponManager.OnSelectedWeaponChanged -= Open;
        EventSystem.current.sendNavigationEvents = true;
    }


    private void SelectUpgradeRow(int selectPointer)
    {
        rowSelectPointer = selectPointer;
        UpgradeRowUI selectedUpgradeRow = upgradeRows[rowSelectPointer];
        selectedUpgradeRow.SetFocused(true);
        foreach (UpgradeRowUI upgradeRow in upgradeRows )
        {
            if (upgradeRow != selectedUpgradeRow)
            {
                upgradeRow.SetFocused(false);
            }
        }

    }

    private void SetWeaponImage(Weapon weaponRef)
    {
        weaponImage.sprite = weaponRef.weaponImage;
        if (weaponRef.name == "Pistol")
        {
            weaponImage.rectTransform.sizeDelta = new Vector2(14f, 17f);
            weaponImage.rectTransform.anchoredPosition = new Vector2(6f, 18f);
        }
        else if (weaponRef.name == "Shotgun")
        {
            weaponImage.rectTransform.sizeDelta = new Vector2(34f, 21f);
            weaponImage.rectTransform.anchoredPosition = new Vector2(6f, 20f);
                
        }
        
    }
}
