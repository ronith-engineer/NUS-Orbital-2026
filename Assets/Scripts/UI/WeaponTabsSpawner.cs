using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTabsSpawner : MonoBehaviour

{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private Transform weaponTabsContainer;
    [SerializeField] private GameObject weaponTabPrefab;
    private List<WeaponTabUI> spawnedTabs = new List<WeaponTabUI>();




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
        }
    }

    private void RefreshHighlights() //Refreshes tabs which are selected in weapon upgrade UI
    {
        foreach (WeaponTabUI tab in spawnedTabs)
        {
            tab.SetSelected(tab.weaponRef == weaponManager.currentSelectedWeapon);
        }
    }
}

