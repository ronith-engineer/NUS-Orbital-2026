using UnityEngine;

public class KeycardManager : MonoBehaviour
{
    public static KeycardManager Instance;

    [SerializeField] private GameObject keycardHUDIcon;

    private bool hasKeycard = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (keycardHUDIcon != null)
            keycardHUDIcon.SetActive(false);
    }

    public void CollectKeycard()
    {
        hasKeycard = true;
        Debug.Log("Keycard collected!");
        if (keycardHUDIcon != null)
            keycardHUDIcon.SetActive(true);
    }

    public bool HasKeycard()
    {
        return hasKeycard;
    }
}