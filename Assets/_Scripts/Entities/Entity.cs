using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(EntitySoundsCommon))]
public abstract class Entity : MonoBehaviour
{
    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public virtual void Attack(Entity target, float damage)
    {
        Health targetHp = target.GetComponent<Health>();
        
        if(targetHp.WasAttackedRecently || targetHp.IsInvulnerable)
            return;

        targetHp.TakeDamage(damage);
    }

    /// <summary>Triggers the stagger animation of the entity.</summary>
    public virtual void Stagger()
    {
        SetBlock(false);
        animator.SetTrigger("Stagger");
        Invoke("SetBlock", animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void SetBlock()
    {
        GetComponentInChildren<Shield>().IsBlocking = true;
    }

    private void SetBlock(bool state)
    {
        GetComponentInChildren<Shield>().IsBlocking = state;
    }
}
