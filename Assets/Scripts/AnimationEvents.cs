using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Entity entity;
    private Knife knife;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        knife = GetComponentInParent<Knife>();
    }

    private void DisableMovementAndJump() => entity.EnableMovementAndJump(false);
    

    private void EnableMovementAndJump() => entity.EnableMovementAndJump(true);
   
    public void EnemyDamageTargets() => entity.DamageTargets();

    public void knifeDamageTargets() => knife.DamageTargets();

}
