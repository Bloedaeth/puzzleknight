using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Enemy : Entity, IFreezable
{
    /// <summary>Is time slowed for this entity.</summary>
    public bool SlowedTime { get; set; }

    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, float damage)
    {
        if(!(target is Player))
            return;

        base.Attack(target, damage);
    }
}
