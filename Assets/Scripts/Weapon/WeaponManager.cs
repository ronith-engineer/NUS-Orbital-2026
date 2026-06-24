using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //for now manually we are putting owned weapons, later on need to be added on pickup.
    [SerializeField] private List<Weapon> ownedWeapons = new List<Weapon>();

    public Weapon currentSelectedWeapon;
    public event Action OnWeaponsChanged;
    public event Action OnSelectedWeaponChanged;

    private void Start()
    {
        OnWeaponsChanged?.Invoke();
        if (ownedWeapons.Count > 0)
        {
            SelectWeapon(ownedWeapons[0]);
        }
    }

    public void SelectWeapon(Weapon weapon)
    {
        if (!ownedWeapons.Contains(weapon)) return;
        if (currentSelectedWeapon != null)
            currentSelectedWeapon.gameObject.SetActive(false);

        currentSelectedWeapon = weapon;
        currentSelectedWeapon.gameObject.SetActive(true);
        OnSelectedWeaponChanged?.Invoke();

    }

    public List<Weapon> GetOwnedWeapons()
    {
        return ownedWeapons;
    }

    //future pickup of weapons function to store in list and refresh weapon upgrade UI
    public void AddWeapon(Weapon weapon)
    {
        if (ownedWeapons.Contains(weapon)) return;

        ownedWeapons.Add(weapon);
        OnWeaponsChanged?.Invoke();
    }

}
