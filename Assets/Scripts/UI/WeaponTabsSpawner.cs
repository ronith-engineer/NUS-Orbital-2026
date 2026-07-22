using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponTabsSpawner : MonoBehaviour

{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private Transform weaponTabsContainer;
    [SerializeField] private GameObject weaponTabPrefab;
    private List<WeaponTabUI> spawnedTabs = new List<WeaponTabUI>();

    public event Action<Weapon> OnTabSelected;




    private void OnEnable()
    {
        weaponManager.OnWeaponsChanged += SpawnWeaponTabs;
        weaponManager.OnSelectedWeaponChanged += RefreshHighlights;

    }

    private void OnDisable()
    {
        weaponManager.OnWeaponsChanged -= SpawnWeaponTabs;
        weaponManager.OnSelectedWeaponChanged -= RefreshHighlights;
    }


    private void SpawnWeaponTabs()
    {
        foreach (Transform child in weaponTabsContainer)
        {
            Destroy(child.gameObject);
        }

        spawnedTabs.Clear();

        foreach (Weapon weapon in weaponManager.GetOwnedWeapons())
        {
            GameObject tabObj = Instantiate(weaponTabPrefab, weaponTabsContainer);
            WeaponTabUI tabUI = tabObj.GetComponent<WeaponTabUI>();
            tabUI.Setup(weapon, weaponManager);
            spawnedTabs.Add(tabUI);
            tabUI.OnTabSelected += OnTabChanged;
        }

        if (spawnedTabs.Count > 0)
            OnTabChanged(spawnedTabs[0].weaponRef);
    }
    private void RefreshHighlights()
    {
        if (WorkbenchPanelUI.Instance == null) return;
        Weapon focusedWeapon = WorkbenchPanelUI.Instance.CurrentWeapon;
        foreach (WeaponTabUI tab in spawnedTabs)
        {
            tab.SetSelected(tab.weaponRef == focusedWeapon);
        }
    }

    private void OnTabChanged(Weapon weapon)
    {
        OnTabSelected?.Invoke(weapon);
        RefreshHighlights();
    }
    public Weapon GetFirstOwnedWeapon()
    {
        if (spawnedTabs.Count == 0) return null;
        return spawnedTabs[0].weaponRef;
    }

    public void RespawnAndReselect()
    {
        SpawnWeaponTabs();
    }
}


