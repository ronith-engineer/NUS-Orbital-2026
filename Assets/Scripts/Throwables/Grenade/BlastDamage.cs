using UnityEngine;

public class BlastDamage : MonoBehaviour
{
    [Header("Blast Details")]
    [SerializeField] private float blastRadius;
    [SerializeField] private float blastMaxDamage;
    private float distanceFromBlast;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;

    [SerializeField] private Transform blastCentre;

    [SerializeField] private GameObject blastGameObject;

    [Header("Noise")]
    [SerializeField] private float blastNoiseRadius = 25f;
    private bool noiseMade = false;

    private void Update()
    {
        Explode();
    }

    private void Explode()
    {
        if (!noiseMade)
        {
            if (NoiseManager.Instance != null)
                NoiseManager.Instance.MakeNoise(blastCentre.position, blastNoiseRadius);
            noiseMade = true;
        }

        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(blastCentre.position, blastRadius, enemyLayerMask | playerLayerMask);
        foreach (Collider2D hitEntity in hitEntities)
        {
            Entity entity = hitEntity.GetComponent<Entity>();
            distanceFromBlast = Vector3.Distance(blastCentre.position, entity.transform.position);
            entity.TakeDamageFromHazard(blastMaxDamage);
        }
    }

    private void EndBlast()
    {
        Destroy(blastGameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(blastCentre.position, blastRadius);
    }
}