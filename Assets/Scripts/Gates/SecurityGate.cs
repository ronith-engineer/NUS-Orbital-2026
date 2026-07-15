using System;
using Unity.Cinemachine;
using UnityEngine;

public class SecurityGate : MonoBehaviour
{
    [Header("Gate Settings")]
    [SerializeField] public bool gateLocked = false;
    [SerializeField] private GameObject linkedGate;
    [SerializeField] private Transform player;

    private SecurityGate linkedGateScript;
    private bool playerNearby = false;
    private bool keypadActive = false;
    private GameObject gateLight;
    private SpriteRenderer gateLightSprite;
    public event Action OnGateUnlocked;

    private void Awake()
    {
        gateLight = transform.parent.Find("GateLight").gameObject;
        gateLightSprite = gateLight.GetComponent<SpriteRenderer>();
        linkedGateScript = linkedGate.GetComponent<SecurityGate>();
    }
    private void Start()
    {
        SetGateColor();
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !gateLocked)
        {
            PlayerTeleporter.TeleportPlayer(player, linkedGate.transform,CameraManager.Instance.MainVCam);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to open door");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            
        }
    }
}