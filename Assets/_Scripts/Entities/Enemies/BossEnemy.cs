using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : Enemy
{
    /// <summary>The swarm to be unleashed when the boss is killed.</summary>
    public GameObject swarm;

    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, int damage)
    {
        base.Attack(target, damage);
    }

    private void OnDisable()
    {
        swarm.SetActive(true);
        foreach(Transform child in swarm.transform)
        {
            child.GetComponent<NavMeshAgent>().speed = 2;
            child.GetComponent<Animator>().speed = 2;
        }
    }
}
