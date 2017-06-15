using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Enemy : Entity
{
    public override void Attack(Entity target, int damage)
    {
        if(!(target is Player))
            return;

        base.Attack(target, damage);
    }
}
