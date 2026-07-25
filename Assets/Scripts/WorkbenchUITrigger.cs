using UnityEngine;

public class WorkbenchUITrigger : MonoBehaviour, ICloseableUI
{
    private BoxCollider2D workbenchCollider;
    [SerializeField] private GameObject weaponUpgradeUI;
    [SerializeField] private Player player;
    private bool playerIsNear;

    [SerializeField] private WeaponManager weaponManager;

    private void Start()
    {
        workbenchCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (playerIsNear)
        {
            if (Input.GetKeyDown(KeyCode.E) && !weaponManager.IsEmpty())
            {
                Debug.Log("E pressed");
                weaponUpgradeUI.SetActive(true);
                MenuManager.Instance.RegisterOpenUI(this);
                player.EnableMovementAndJump(false);
            }
        }
        else
        {
            if (weaponUpgradeUI.activeSelf)
                CloseUI();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            playerIsNear = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = false;

        }
    }

    public void CloseUI()
    {
        weaponUpgradeUI.SetActive(false);
        player.EnableMovementAndJump(true);
        MenuManager.Instance.UnregisterOpenUI(this);
    }
}
