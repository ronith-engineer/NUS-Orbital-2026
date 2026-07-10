using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WeaponHUD : MonoBehaviour
{
    private float currentAmmo;
    private float reserveAmmo;

    [SerializeField] private Weapon weapon;
    [SerializeField] private TextMeshProUGUI ammoCounter;


    void Update()
    {
        currentAmmo = weapon.currentAmmo;
        reserveAmmo = weapon.reserveAmmo;
        ammoCounter.text = $"{currentAmmo} | {reserveAmmo}";

    }
}
