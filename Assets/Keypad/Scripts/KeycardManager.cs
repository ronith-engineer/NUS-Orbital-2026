using UnityEngine;

public class KeycardManager : MonoBehaviour
{
    public static KeycardManager Instance;


    private bool hasKeycard = false;

    private void Awake()
    {
        Instance = this;
    }


    public void CollectKeycard()
    {
        hasKeycard = true;

    }

    public bool HasKeycard()
    {
        return hasKeycard;
    }
}