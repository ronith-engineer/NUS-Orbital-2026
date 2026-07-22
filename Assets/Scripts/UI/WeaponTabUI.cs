using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTabUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;


    public Weapon weaponRef;
    private WeaponManager weaponManager;
    [SerializeField] private GameObject selectedHighlight;

    public event Action<Weapon> OnTabSelected;


    public void Setup(Weapon weapon, WeaponManager manager)
    {

        weaponRef = weapon;
        iconImage.sprite = weapon.icon;
        weaponManager = manager;
        Debug.Log($"[WeaponTabUI] Setup called for {weaponRef.weaponName}, adding listener. Button null? {button == null}");

        button.onClick.AddListener(OnClick);

        Debug.Log($"[WeaponTabUI] Listener count after add: {button.onClick.GetPersistentEventCount()}");

        //SetSelected(weaponManager.currentSelectedWeapon == weaponRef);


    }

    private void OnClick()
    {
        Debug.Log("Tab clicked: " + weaponRef.weaponName);

        //weaponManager.SelectWeapon(weaponRef);
        OnTabSelected?.Invoke(weaponRef);
    }

    public void SetSelected(bool isSelected)
    {
        selectedHighlight.SetActive(isSelected);
    }


}