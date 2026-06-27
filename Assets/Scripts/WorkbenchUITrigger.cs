using UnityEngine;

public class WorkbenchUITrigger : MonoBehaviour
{
    private BoxCollider2D workbenchCollider;
    [SerializeField] private GameObject weaponUpgradeUI;
    [SerializeField] private Player player;
    private bool playerIsNear;

    private void Start()
    {
        workbenchCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (playerIsNear)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E pressed");
                weaponUpgradeUI.SetActive(true);
                player.EnableMovementAndJump(false);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                weaponUpgradeUI.SetActive(false);
                player.EnableMovementAndJump(true);
            }
        }
        else
        {
            weaponUpgradeUI.SetActive(false);
            player.EnableMovementAndJump(true);
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
}
