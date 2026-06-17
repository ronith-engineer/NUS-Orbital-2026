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

    private float fireStartTime;
    private float damageTimestamp;
    private Dictionary<Collider2D, float> entityDamageTimestamps = new Dictionary<Collider2D, float>();

    private void Start()
    {
        fireStartTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup - fireStartTime > fireTimeDuration)
        {
            Destroy(gameObject);
        }
    }

    // FixedUpdate is called once per fixed frame
    private void FixedUpdate()
    {
        Collider2D[] hitEntities = Physics2D.OverlapBoxAll(fireCentre.position, new Vector2(fireLength,fireHeight), 0f, playerLayerMask | enemyLayerMask);
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
        Gizmos.DrawWireCube(fireCentre.position, new Vector3(fireLength, fireHeight,0f));

    }

}
