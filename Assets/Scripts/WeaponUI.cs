using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pistolUI;
    [SerializeField] private GameObject knifeUI;
    [SerializeField] private GameObject shotgunUI;
    [SerializeField] private GameObject molotovUI;
    [SerializeField] private GameObject grenadeUI;

    [Header("Weapon References")]
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject molotov;
    [SerializeField] private GameObject grenade;

    // Update is called once per frame
    void Update()
    {
        if (pistol != null && pistol.activeSelf) // If the pistol is active, show the pistol UI and hide the knife UI
        {
            pistolUI.SetActive(true);
            knifeUI.SetActive(false);
            shotgunUI.SetActive(false);
            molotovUI.SetActive(false);
            grenadeUI.SetActive(false);
        }
        else if (knife != null && knife.activeSelf) // If the knife is active, show the knife UI and hide the pistol UI
        {
            knifeUI.SetActive(true);
            pistolUI.SetActive(false);
            shotgunUI.SetActive(false);
            molotovUI.SetActive(false);
            grenadeUI.SetActive(false);
        }
        else if (shotgun != null && shotgun.activeSelf )
        {
            shotgunUI.SetActive(true);
            pistolUI.SetActive(false);
            knifeUI.SetActive(false);
            molotovUI.SetActive(false);
            grenadeUI.SetActive(false);
        }
        else if (molotov != null && molotov.activeSelf )
        {
            molotovUI.SetActive(true);
            shotgunUI.SetActive(false);
            pistolUI.SetActive (false);
            knifeUI.SetActive(false);
            grenadeUI.SetActive(false);
        }
        else if (grenade != null && grenade.activeSelf )
        {
            grenadeUI.SetActive(true);
            shotgunUI.SetActive(false);
            pistolUI.SetActive(false);
            knifeUI.SetActive(false);
            molotovUI.SetActive(false);
        }
        else
        {
            grenadeUI.SetActive(false);
            shotgunUI.SetActive(false);
            pistolUI.SetActive(false);
            knifeUI.SetActive(false);
            molotovUI.SetActive(false);
        }
}
}
