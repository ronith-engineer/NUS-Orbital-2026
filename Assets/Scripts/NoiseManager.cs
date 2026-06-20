using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance;

    [Header("Noise Settings")]
    [SerializeField] private float maxNoise = 100f;
    [SerializeField] private float noiseRiseSpeed = 80f;
    private float currentNoise;
    private float targetNoise;
    private bool noiseActiveThisFrame;

    [Header("UI")]
    [SerializeField] private Image[] noiseBars;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentNoise = 0f;
        targetNoise = 0f;
        UpdateBars();
    }

    private void LateUpdate()
    {
        if (noiseActiveThisFrame)
        {
            if (currentNoise < targetNoise)
            {
                currentNoise += noiseRiseSpeed * Time.deltaTime;
                currentNoise = Mathf.Min(currentNoise, targetNoise);
            }
            else
            {
                currentNoise = targetNoise;
            }
        }
        else
        {
            currentNoise = 0f;
            targetNoise = 0f;
        }

        UpdateBars();
        noiseActiveThisFrame = false;
        targetNoise = 0f;
    }

    private void UpdateBars()
    {
        int activeBars = Mathf.FloorToInt(currentNoise / 10f);
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
        noiseActiveThisFrame = true;
        if (amount > targetNoise)
            targetNoise = amount;
    }

    public float GetCurrentNoise()
    {
        return currentNoise;
    }
}