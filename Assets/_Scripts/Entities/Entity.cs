using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(EntitySoundsCommon))]
public abstract class Entity : MonoBehaviour
{
    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public virtual void Attack(Entity target, int damage)
    {
        Health targetHp = target.GetComponent<Health>();
        
        if(targetHp.WasAttackedRecently)
            return;

        targetHp.TakeDamage(damage);
    }
}
