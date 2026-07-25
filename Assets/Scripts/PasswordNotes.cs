using System;
using UnityEngine;

public class PasswordNotes : MonoBehaviour, ICloseableUI
{
    [SerializeField] private GameObject noteCanvas;
    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                noteCanvas.SetActive(true);
                MenuManager.Instance.RegisterOpenUI(this);
                Player.Instance.EnableMovementAndJump(false);
            }
        }
        else
        {
            if (noteCanvas.activeSelf)
                CloseUI();
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    public void CloseUI()
    {
        noteCanvas.SetActive(false);
        Player.Instance.EnableMovementAndJump(true);
        MenuManager.Instance.UnregisterOpenUI(this);
    }


}
