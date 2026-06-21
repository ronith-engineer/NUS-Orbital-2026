using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance;

    [Header("Noise Settings")]
    [SerializeField] private float maxNoise = 100f;
    [SerializeField] private float noiseRiseSpeed = 150f;

    [Header("UI")]
    [SerializeField] private Image[] noiseBars;

    private float movementNoise;
    private float movementTarget;
    private bool movementActiveThisFrame;

    private float shootDisplayNoise;
    private float shootDisplayTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        movementNoise = 0f;
        movementTarget = 0f;
        shootDisplayNoise = 0f;
        shootDisplayTimer = 0f;
        UpdateBars(0f);
    }

    private void LateUpdate()
    {
        HandleMovementNoise();
        HandleShootNoise();

        float finalNoise = Mathf.Max(movementNoise, shootDisplayNoise);
        UpdateBars(finalNoise);

        movementActiveThisFrame = false;
        movementTarget = 0f;
    }

    private void HandleMovementNoise()
    {
        if (movementActiveThisFrame)
        {
            if (movementNoise < movementTarget)
            {
                movementNoise += noiseRiseSpeed * Time.deltaTime;
                if (movementNoise > movementTarget)
                    movementNoise = movementTarget;
            }
            else
            {
                movementNoise = movementTarget;
            }
        }
        else
        {
            movementNoise = 0f;
            movementTarget = 0f;
        }
    }

    private void HandleShootNoise()
    {
        if (shootDisplayTimer > 0f)
        {
            shootDisplayTimer -= Time.deltaTime;
            if (shootDisplayTimer <= 0f)
            {
                shootDisplayNoise = 0f;
                shootDisplayTimer = 0f;
            }
        }
    }

    private void UpdateBars(float noise)
    {
        int activeBars = Mathf.FloorToInt(noise / 10f);
        activeBars = Mathf.Clamp(activeBars, 0, noiseBars.Length);

        for (int i = 0; i < noiseBars.Length; i++)
        {
            if (i < activeBars)
                noiseBars[i].enabled = true;
            else
                noiseBars[i].enabled = false;
        }
    }

    public void SetNoise(float amount)
    {
        movementActiveThisFrame = true;
        movementTarget = amount;
    }

    public void SetShootNoise(float amount, float duration)
    {
        shootDisplayNoise = amount;
        shootDisplayTimer = duration;
    }

    public float GetCurrentNoise()
    {
        return Mathf.Max(movementNoise, shootDisplayNoise);
    }
}