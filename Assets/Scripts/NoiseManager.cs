using System.Collections.Generic;
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

    [Header("Enemy Detection")]
    [SerializeField] private LayerMask enemyLayerMask;

    [Header("Debug Gizmos")]
    [SerializeField] private bool showNoiseGizmos = true;
    [SerializeField] private float gizmoLifetime = 1f;

    private float movementNoise;
    private float movementTarget;
    private bool movementActiveThisFrame;

    private float shootDisplayNoise;
    private float shootDisplayTimer;

    private class NoiseEvent
    {
        public Vector2 position;
        public float radius;
        public float timeCreated;
    }

    private List<NoiseEvent> recentNoises = new List<NoiseEvent>();

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

    public void MakeNoise(Vector2 position, float radius)
    {
        if (showNoiseGizmos)
        {
            recentNoises.Add(new NoiseEvent
            {
                position = position,
                radius = radius,
                timeCreated = Time.time
            });
        }

        Collider2D[] enemies = Physics2D.OverlapBoxAll(position, new Vector2(radius,5f),0f, enemyLayerMask);

        foreach (Collider2D col in enemies)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnNoiseHeard(position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showNoiseGizmos) return;
        if (!Application.isPlaying) return;

        recentNoises.RemoveAll(n => Time.time - n.timeCreated > gizmoLifetime);

        foreach (NoiseEvent noise in recentNoises)
        {
            float age = (Time.time - noise.timeCreated) / gizmoLifetime;
            Gizmos.color = new Color(1f, 0.5f, 0f, 1f - age);
            Gizmos.DrawWireCube(noise.position, new Vector2(noise.radius,5f));
        }
    }
}