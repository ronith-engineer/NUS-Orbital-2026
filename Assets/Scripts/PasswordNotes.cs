using UnityEngine;

public class PasswordNotes : MonoBehaviour
{
    [SerializeField] private GameObject noteCanvas;

    private bool isPlayerNearby = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                noteCanvas.SetActive(true);
                Player.Instance.EnableMovementAndJump(false);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                noteCanvas.SetActive(false);
                Player.Instance.EnableMovementAndJump(true);
            }
        }
        else
        {
            noteCanvas.SetActive(false);

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


}
