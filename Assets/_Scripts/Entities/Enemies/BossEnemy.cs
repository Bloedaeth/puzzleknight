using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class BossEnemy : Enemy
{
    private Health hp;
    private AICharacterControl ai;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private const float SCALE_MULT = 10f;
    private int attackHashStage1;
    private int attackHashStage2;
    private int stage = 1;

    private void Awake()
    {
        hp = GetComponent<Health>();
        ai = GetComponent<AICharacterControl>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();
        attackHashStage1 = Animator.StringToHash("Base Layer.Attack Stage 1");
        attackHashStage2 = Animator.StringToHash("Base Layer.Attack Stage 2");

        agent.stoppingDistance = 1.5f * SCALE_MULT;
        animator.SetInteger("Stage", 1);
    }

    private void Update()
    {
        if(hp.HealthRemaining <= hp.InitialAndMaxHealth / 2f && stage != 2)
        {
            stage = 2;
            animator.SetInteger("Stage", 2);
        }

        if(SlowedTime)
        {
            animator.speed = TimeFreeze.FROZEN_TIME_SCALE;
            agent.speed = TimeFreeze.FROZEN_TIME_SCALE;
        }
        else
        {
            animator.speed = 1f;
            agent.speed = 1f;
        }
        
        float dist = Mathf.Abs(Vector3.Distance(transform.position, player.position));
        if(dist <= agent.stoppingDistance)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if(state.fullPathHash != attackHashStage1 && state.fullPathHash != attackHashStage2)
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
