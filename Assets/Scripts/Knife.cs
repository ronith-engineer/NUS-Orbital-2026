using UnityEngine;

public class Knife : MonoBehaviour
{
    private Animator anim;
    public int knifeCurrentDurability = 3;

    private Transform attackPoint;

    [SerializeField] private float attackRadius;

    private LayerMask enemyLayerMask;
    private Player player;

    private float attackDamage = 1f;
    [SerializeField] private float knockbackForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        player = GetComponentInParent<Player>();
        attackPoint = transform.Find("AttackPoint").GetComponent<Transform>();
        enemyLayerMask = LayerMask.GetMask("Enemy");


    }
    private void Update()
    {
        if (knifeCurrentDurability <= 0) // If the knife has no durability left, disable the knife GameObject
        {
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && knifeCurrentDurability > 0) // If the left mouse button is pressed and the knife has durability, perform an attack

        {
            knifeCurrentDurability--;
            anim.SetTrigger("knifeAttack");
        }

    }

    public void DamageTargets()
    {
        // Set knockback direction based on facing direction
        Collider2D[] targetColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayerMask);

        foreach (Collider2D target in targetColliders)
        {
            Entity entityTarget = target.GetComponent<Entity>();
            entityTarget.TakeDamageFromEntity(player.facingRight, attackDamage, knockbackForce);

        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
