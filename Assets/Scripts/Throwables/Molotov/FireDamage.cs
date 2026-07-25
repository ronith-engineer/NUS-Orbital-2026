using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    [Header("Layer Masks")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;

    [SerializeField] private Transform fireCentre;

    [Header("Fire Settings")]
    [SerializeField] private float fireLength;
    [SerializeField] private float fireHeight;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float fireTimeDuration;

    [Header("Noise")]
    [SerializeField] private float fireNoiseRadius = 20f;
    [SerializeField] private float noiseInterval = 1f;
    [SerializeField] private bool showNoiseGizmo = true;
    private float noiseTimer;

    private float fireStartTime;
    private Dictionary<Collider2D, float> entityDamageTimestamps = new Dictionary<Collider2D, float>();

    private void Start()
    {
        fireStartTime = Time.realtimeSinceStartup;

        if (NoiseManager.Instance != null)
            NoiseManager.Instance.MakeNoise(fireCentre.position, fireNoiseRadius);

        noiseTimer = noiseInterval;
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup - fireStartTime > fireTimeDuration)
        {
            Destroy(gameObject);
            return;
        }

        noiseTimer -= Time.deltaTime;
        if (noiseTimer <= 0f)
        {
            if (NoiseManager.Instance != null)
                NoiseManager.Instance.MakeNoise(fireCentre.position, fireNoiseRadius);
            noiseTimer = noiseInterval;
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] hitEntities = Physics2D.OverlapBoxAll(fireCentre.position, new Vector2(fireLength, fireHeight), 0f, playerLayerMask | enemyLayerMask);
        foreach (Collider2D entityCollider in hitEntities)
        {
            if (!entityDamageTimestamps.ContainsKey(entityCollider))
            {
                Entity entity = entityCollider.GetComponent<Entity>();
                entity.TakeDamageFromHazard(damage);
                entityDamageTimestamps[entityCollider] = Time.time;
            }
            else if (Time.time - entityDamageTimestamps[entityCollider] > 0.5f)
            {
                Entity entity = entityCollider.GetComponent<Entity>();
                entity.TakeDamageFromHazard(damage);
                entityDamageTimestamps[entityCollider] = Time.time;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (fireCentre == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(fireCentre.position, new Vector3(fireLength, fireHeight, 0f));

        if (!showNoiseGizmo) return;

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.8f);
        Gizmos.DrawWireSphere(fireCentre.position, fireNoiseRadius);
    }
}