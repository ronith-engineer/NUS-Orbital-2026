using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Entity entity;
    private Knife knife;
    private Weapon weapon;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        knife = GetComponentInParent<Knife>();
        weapon = GetComponentInParent<Weapon>();
    }

    private void DisableMovementAndJump() => entity.EnableMovementAndJump(false);
    

    private void EnableMovementAndJump() => entity.EnableMovementAndJump(true);
   
    public void EnemyDamageTargets() => entity.DamageTargets();

    public void KnifeDamageTargets() => knife.DamageTargets();

    public void DisableReloadAndShoot() => weapon.EnableReloadAndShoot(false);

    public void EnableReloadAndShoot() => weapon.EnableReloadAndShoot(true);
}
