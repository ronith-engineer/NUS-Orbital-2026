using UnityEngine;

public class SecurityGate : MonoBehaviour
{
    [Header("Gate Settings")]
    [SerializeField] private GameObject keypadObject; // drag your Keypad prefab here

    private bool playerNearby = false;
    private bool keypadActive = false;
    private bool gateOpened = false;

    private void Start()
    {
        if (keypadObject != null)
            keypadObject.SetActive(false); // hidden at start
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !gateOpened)
        {
            keypadActive = !keypadActive;
            keypadObject.SetActive(keypadActive);
        }
    }

    // Called by Keypad's onAccessGranted UnityEvent
    public void OpenGate()
    {
        gateOpened = true;
        keypadActive = false;
        if (keypadObject != null)
            keypadObject.SetActive(false);

        transform.position += new Vector3(0, 5, 0);
        Debug.Log("Gate is now open!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to open keypad");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            keypadActive = false;
            if (keypadObject != null)
                keypadObject.SetActive(false);
        }
    }
}