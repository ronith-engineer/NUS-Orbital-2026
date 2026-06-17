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


    private void Update()
    {
        Explode();
    }
    private void Explode()
    {
        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(blastCentre.position, blastRadius, enemyLayerMask | playerLayerMask);
        foreach (Collider2D hitEntity in hitEntities)
        {
            Entity entity = hitEntity.GetComponent<Entity>();
            distanceFromBlast = Vector3.Distance(blastCentre.position, entity.transform.position);
            Debug.Log(distanceFromBlast);
            //float damage = Mathf.Lerp(blastMaxDamage, 0f, distanceFromBlast/blastRadius);
            Debug.Log(blastMaxDamage);
            entity.TakeDamageFromHazard(blastMaxDamage);
        }
    }

    private void EndBlast()
    {
        Destroy(blastGameObject);
        Debug.Log("Blast Ended");

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(blastCentre.position, blastRadius);
    }
}
