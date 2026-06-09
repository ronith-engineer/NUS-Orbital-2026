using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pistolUI;
    [SerializeField] private GameObject knifeUI;
    [SerializeField] private GameObject shotgunUI;

    [Header("Weapon References")]
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject shotgun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pistol.activeSelf) // If the pistol is active, show the pistol UI and hide the knife UI
        {
            pistolUI.SetActive(true);
            knifeUI.SetActive(false);
            shotgunUI.SetActive(false);
        }
        else if (knife.activeSelf) // If the knife is active, show the knife UI and hide the pistol UI
        {
            knifeUI.SetActive(true);
            pistolUI.SetActive(false);
            shotgunUI.SetActive(false);
        }
        else if (shotgun.activeSelf)
        {
            shotgunUI.SetActive(true);
            pistolUI.SetActive(false);
            knifeUI.SetActive(false);
        }
    }
}
