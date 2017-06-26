using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Enemy : Entity
{
    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, int damage)
    {
        if(!(target is Player))
            return;

        base.Attack(target, damage);
    }
}
