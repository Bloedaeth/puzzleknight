using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class GruntEnemy : Enemy
{
    private AICharacterControl ai;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private int attackHash;

    private float initialAgentSpeed;
    private float initialAnimatorSpeed;

    private void Awake()
    {
        ai = GetComponent<AICharacterControl>();
        agent = GetComponent<NavMeshAgent>();
        initialAgentSpeed = agent.speed;
        player = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();
        initialAnimatorSpeed = animator.speed;
        attackHash = Animator.StringToHash("Base Layer.Attack");

        agent.stoppingDistance = 1.5f;
    }

    private void Update()
    {
        if(SlowedTime)
        {
            animator.speed = TimeFreeze.FROZEN_TIME_SCALE;
            agent.speed = TimeFreeze.FROZEN_TIME_SCALE;
        }
        else
        {
            animator.speed = initialAnimatorSpeed;
            agent.speed = initialAgentSpeed;
        }

        if(ai.target == null && Mathf.Abs(Vector3.Distance(transform.position, player.position)) < 10f)
            ai.SetTarget(player);

        float dist = Mathf.Abs(Vector3.Distance(transform.position, player.position));
        if(dist < agent.stoppingDistance)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if(state.fullPathHash != attackHash)
            {
                transform.LookAt(player); //AI tends to attack at air because wrong rotation
                animator.SetTrigger("Attack");
            }
        }
    }

    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, int damage)
    {
        base.Attack(target, damage);
    }
}
