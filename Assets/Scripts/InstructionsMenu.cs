using UnityEngine;

public class InstructionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject movementControlsMenu;
    [SerializeField] private GameObject weaponsControlMenu;
    [SerializeField] private GameObject inventoryControlsMenu;
    [SerializeField] private GameObject craftingSystemMenu;
    [SerializeField] private GameObject weaponUpgradeSystemMenu;
    [SerializeField] private GameObject interactablesMenu;  
    public void ShowMovementControlsMenu()
    {
        movementControlsMenu.SetActive(true);
    }
    public void HideMovementControlsMenu()
    {
        movementControlsMenu.SetActive(false);
    }

    public void ShowWeaponsControlMenu()
    {
        weaponsControlMenu.SetActive(true);
    }

    public void HideWeaponsControlMenu()
    {
        weaponsControlMenu.SetActive(false);
    }

    public void ShowInventoryControlsMenu()
    {
        inventoryControlsMenu.SetActive(true);
    }

    public void HideInventoryControlsMenu()
    {
        inventoryControlsMenu.SetActive(false);
    }

    public void ShowCraftingSystemMenu()
    {
        craftingSystemMenu.SetActive(true);
    }

    public void HideCraftingSystemMenu()
    {
        craftingSystemMenu.SetActive(false);
    }

    public void ShowWeaponUpgradeSystemMenu()
    {
        weaponUpgradeSystemMenu.SetActive(true);
    }

    public void HideWeaponUpgradeSystemMenu()
    {
        weaponUpgradeSystemMenu.SetActive(false);
    }

    public void ShowInteractablesMenu()
    {
        interactablesMenu.SetActive(true);
    }

    public void HideInteractablesMenu()
    {
        interactablesMenu.SetActive(false);
    }
}
