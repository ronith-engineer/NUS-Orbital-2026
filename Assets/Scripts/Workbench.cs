using UnityEngine;

public class Workbench : MonoBehaviour
{
    private BoxCollider2D playerDetectionCollider;

    void Start()
    {
        playerDetectionCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Press E to use workbench");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Press E to use workbench");
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }
}
