using UnityEngine;

public class SecurityGate : MonoBehaviour
{
    [Header("Gate Settings")]
    [SerializeField] private GameObject keypadObject;
    [SerializeField] private GameObject overlayObject;
    [SerializeField] private Vector3 smallScale = new Vector3(0.3f, 0.3f, 0.3f);
    [SerializeField] private Vector3 bigScale = new Vector3(2f, 2f, 2f);
    [SerializeField] private Vector3 bigPosition = new Vector3(0, 0, 0);
    [SerializeField] private float zoomSpeed = 5f;

    private bool playerNearby = false;
    private bool keypadActive = false;
    private bool gateOpened = false;
    private bool isAnimating = false;
    private Vector3 smallPosition;

    private void Start()
    {
        if (keypadObject != null)
        {
            smallPosition = keypadObject.transform.position;
            keypadObject.SetActive(true);
            keypadObject.transform.localScale = smallScale;
        }
        if (overlayObject != null)
            overlayObject.SetActive(false);
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !gateOpened && !isAnimating)
        {
            if (!keypadActive)
                OpenKeypad();
            else
                CloseKeypad();
        }
    }

    private void OpenKeypad()
    {
        keypadActive = true;
        if (overlayObject != null) overlayObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ScaleKeypad(bigScale, bigPosition));
    }

    private void CloseKeypad()
    {
        keypadActive = false;
        if (overlayObject != null) overlayObject.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(ScaleKeypad(smallScale, smallPosition));
    }

    public void OpenGate()
    {
        gateOpened = true;
        StopAllCoroutines();
        if (overlayObject != null) overlayObject.SetActive(false);
        if (keypadObject != null) keypadObject.SetActive(false);
        transform.position += new Vector3(0, 5, 0);
        Debug.Log("Gate opened!");
    }
    private System.Collections.IEnumerator ScaleKeypad(Vector3 targetScale, Vector3 targetPosition)
    {
        isAnimating = true;
        Vector3 startScale = keypadObject.transform.localScale;
        Vector3 startPosition = keypadObject.transform.position;
        float elapsed = 0f;
        float duration = 0.3f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            keypadObject.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            keypadObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        keypadObject.transform.localScale = targetScale;
        keypadObject.transform.position = targetPosition;
        isAnimating = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to use keypad");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            if (keypadActive) CloseKeypad();
        }
    }
}