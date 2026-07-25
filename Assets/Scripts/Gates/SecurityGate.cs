using System;
using UnityEngine;

public class SecurityGate : Pathways
{
    [SerializeField] public bool gateLocked = false;
    [SerializeField] private bool isFinalGate = false;
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
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PlayerTeleporter.TeleportPlayer(player, linkedPathway.transform,CameraManager.Instance.MainVCam);
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
