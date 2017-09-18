using UnityEngine;

public class ToughEnemy : Enemy
{
    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, int damage)
    {
        base.Attack(target, damage);
    }
}
