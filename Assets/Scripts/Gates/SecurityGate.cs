using System;
using UnityEngine;

public class SecurityGate : Pathways
{
    [SerializeField] public bool gateLocked = false;
    [SerializeField] private bool isFinalGate = false;
    [SerializeField] private GameObject GameOverScreen;
    private SecurityGate linkedGateScript;
    private GameObject gateLight;
    private SpriteRenderer gateLightSprite;
    

    private void Awake()
    {
        gateLight = transform.Find("GateLight").gameObject;
        gateLightSprite = gateLight.GetComponent<SpriteRenderer>();
        linkedGateScript = linkedPathway.GetComponent<SecurityGate>();
    }

    private void Start()
    {
        SetGateColor();
    }

    protected override void Update()
    {
        if (!isFinalGate && playerNearby && Input.GetKeyDown(KeyCode.E) && !gateLocked)
        {
            if (gateLocked)
            {
                Debug.Log("Gate is locked. Use the keypad.");
                return;
            }
            PlayerTeleporter.TeleportPlayer(player, linkedPathway.transform, CameraManager.Instance.MainVCam);
        }
        else if (isFinalGate && playerNearby && Input.GetKeyDown(KeyCode.E) && !gateLocked)
        {
            MenuManager.Instance.ShowGameOverWin();
        }
    }

    public void OpenGate()
    {
        UnlockThisGate();
        linkedGateScript.UnlockThisGate();
    }

    private void UnlockThisGate()
    {
        gateLocked = false;
        SetGateColor();
    }

    private void SetGateColor()
    {
        if (gateLocked)
        {
            gateLightSprite.color = new Color32(168, 13, 0, 255);
        }
        else
        {
            gateLightSprite.color = new Color32(0, 168, 13, 255);
        }
    }
}