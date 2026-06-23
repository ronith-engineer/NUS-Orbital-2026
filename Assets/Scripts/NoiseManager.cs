using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance;

    [Header("Noise Settings")]
    [SerializeField] private float maxNoise = 100f;
    [SerializeField] private float noiseDecayRate = 15f;
    private float currentNoise;

    [Header("UI")]
    [SerializeField] private Image[] noiseBars;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentNoise = 0f;
        UpdateBars();
    }

    private void Update()
    {
        UpdateBars();
        currentNoise = 0f;
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
        currentNoise = amount;
    }

    public float GetCurrentNoise()
    {
        return currentNoise;
    }
}