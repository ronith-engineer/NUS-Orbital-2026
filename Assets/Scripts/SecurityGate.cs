using UnityEngine;

public class SecurityGate : MonoBehaviour
{
    [Header("Gate Settings")]
    [SerializeField] private string correctCode = "1234";

    private bool playerNearby = false;
    private bool keypadActive = false;
    private string enteredCode = "";

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            keypadActive = true;
            enteredCode = "";
            Debug.Log("Enter code (press Enter to confirm):");
        }

        if (keypadActive)
        {
            // Type numbers
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c) && enteredCode.Length < 4)
                {
                    enteredCode += c;
                    Debug.Log("Code so far: " + enteredCode);
                }

                // Backspace to delete
                if (c == '\b' && enteredCode.Length > 0)
                    enteredCode = enteredCode.Substring(0, enteredCode.Length - 1);

                // Enter to submit
                if (c == '\n' || c == '\r')
                    TryCode();
            }
        }
    }

    private void TryCode()
    {
        if (enteredCode == correctCode)
        {
            Debug.Log("Correct! Gate opening!");
            keypadActive = false;
            OpenGate();
        }
        else
        {
            Debug.Log("Wrong code: " + enteredCode + " Try again!");
            enteredCode = "";
        }
    }

   
    private void OpenGate()
    {
        transform.position += new Vector3(0, 5, 0);
        Debug.Log("Gate is now open!");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to enter code");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            keypadActive = false;
        }
    }
}