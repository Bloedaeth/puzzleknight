using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(EntitySoundsCommon))]
public abstract class Entity : MonoBehaviour
{
    public virtual void Attack(Entity target, int damage)
    {
        Health targetHp = target.GetComponent<Health>();
        
        if(targetHp.WasAttackedRecently)
            return;

        targetHp.TakeDamage(damage);
    }
}
