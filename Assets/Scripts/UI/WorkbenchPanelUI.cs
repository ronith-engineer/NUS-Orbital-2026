using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkbenchPanelUI : MonoBehaviour
{
    public static WorkbenchPanelUI Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private Image weaponImage;
    [SerializeField] private WeaponManager weaponManager;

    [Header("Upgrade Rows")]
    [SerializeField] public UpgradeRowUI upgradeRow1;
    [SerializeField] public UpgradeRowUI upgradeRow2;

    [Header("Weapon Upgrades")]
    [SerializeField] private WeaponUpgrade weaponUpgrade1;
    [SerializeField] private WeaponUpgrade weaponUpgrade2;

    public Weapon CurrentWeapon { get; private set; }

    [SerializeField] private WeaponTabsSpawner weaponTabsSpawner;
    public int rowSelectPointer;
    private int countUpgradeRows;

    private void Awake()
    {
        //using singleton pattern to avoid any possible duplication of the workbench panel ui in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private List<UpgradeRowUI> upgradeRows => new List<UpgradeRowUI>() { upgradeRow1, upgradeRow2};

    private void Start()
    {
        countUpgradeRows = upgradeRows.Count;
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



   

    public void Open(Weapon weapon)
    {
        CurrentWeapon = weapon;
        weaponNameText.text = weapon.weaponName;
        SetWeaponImage(weapon);
        upgradeRow1.Setup(weapon, weaponUpgrade1);
        upgradeRow2.Setup(weapon, weaponUpgrade2);
        SelectUpgradeRow(0);

    }

    private void OnEnable()
    {
        if (weaponManager == null) return;
        weaponTabsSpawner.OnTabSelected += Open;
        EventSystem.current.sendNavigationEvents = false;
        rowSelectPointer = 0;

        weaponTabsSpawner.RespawnAndReselect();
    }

    private void OnDisable()
    {
        weaponTabsSpawner.OnTabSelected -= Open;
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
            weaponImage.rectTransform.sizeDelta = new Vector2(30.1f,36.5f);
            weaponImage.rectTransform.anchoredPosition = new Vector2(7f, 12.04f);
        }
        else if (weaponRef.name == "Shotgun")
        {
            weaponImage.rectTransform.sizeDelta = new Vector2(27.4f, 17f);
            weaponImage.rectTransform.anchoredPosition = new Vector2(5.53f, 17.1f);
                
        }
        
    }
}
