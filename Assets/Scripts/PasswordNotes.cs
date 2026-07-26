using System;
using UnityEngine;

public class PasswordNotes : MonoBehaviour, ICloseableUI
{
    [SerializeField] private GameObject noteCanvas;
    private bool isPlayerNearby = false;

    [Header("Slow Motion")]
    [SerializeField] private float slowMotionScale = 0.1f;
    [SerializeField] private bool useSlowMotion = true;

    void Update()
    {
        if (isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenUI();
            }
        }
        else
        {
            if (noteCanvas.activeSelf)
            {
                CloseUI();
            }
                
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
        SetSlowMotion(false);
    }

    public void OpenUI()
    {
        noteCanvas.SetActive(true);
        Player.Instance.EnableMovementAndJump(false);
        MenuManager.Instance.RegisterOpenUI(this);
        SetSlowMotion(true);
    }

    private void SetSlowMotion(bool slow)
    {
        if (!useSlowMotion) return;

        if (slow)
        {
            Time.timeScale = slowMotionScale;
            Time.fixedDeltaTime = 0.02f * slowMotionScale;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }


}
