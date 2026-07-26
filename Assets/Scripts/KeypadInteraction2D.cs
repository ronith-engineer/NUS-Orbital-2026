using UnityEngine;
namespace NavKeypad
{
    public class KeypadInteraction2D : MonoBehaviour
    {
        private Camera cam;
        [SerializeField] private LayerMask buttonLayerMask;

        private void Awake() => cam = Camera.main;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, buttonLayerMask);

                Debug.Log("Hit collider: " + (hit.collider != null ? hit.collider.name : "NONE"));

                if (hit.collider != null && hit.collider.TryGetComponent(out KeypadButton keypadButton))
                {
                    keypadButton.PressButton();
                }
            }
        }
    }
}