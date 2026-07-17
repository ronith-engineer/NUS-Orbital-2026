using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineCamera mainVCam;
    public CinemachineCamera MainVCam => mainVCam;
    private CinemachineConfiner2D confiner;

    private void Awake()
    {
        //using singleton pattern to avoid any possible duplication of the camera manager in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        confiner = mainVCam.GetComponent<CinemachineConfiner2D>();
    }

    public void SwitchConfiner(Collider2D newBounds)
    {
        if (confiner == null) return;
        confiner.BoundingShape2D = newBounds;
        confiner.InvalidateBoundingShapeCache();
    }
}

