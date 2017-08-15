using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class RangedEnemy : Enemy
{
    private AICharacterControl ai;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private int attackHash;
    private float launchCooldown = 2.5f;

    private void Awake()
    {
        ai = GetComponent<AICharacterControl>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();

        attackHash = Animator.StringToHash("Base Layer.Attack");

        agent.stoppingDistance = 15f;
    }

    private void Update()
    {
        launchCooldown -= Time.deltaTime;
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

        if(ai.target == null && Mathf.Abs(Vector3.Distance(transform.position, player.position)) < 20f)
            ai.SetTarget(player);

        float dist = Mathf.Abs(Vector3.Distance(transform.position, player.position));
        if(dist <= agent.stoppingDistance)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if(state.fullPathHash != attackHash)
            {
                transform.LookAt(player); //AI tends to attack at air because wrong rotation
                animator.SetTrigger("Attack");
                LaunchProjectile();
            }
        }
    }

    private void LaunchProjectile()
    {
        if(launchCooldown <= 0)
        {
            Projectile projectile = ObjectPooler.main.GetPooledObject().GetComponent<Projectile>();
            projectile.transform.position = transform.position + transform.forward + Vector3.up;
            projectile.transform.forward = transform.forward;
            projectile.Self = this;
            projectile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            projectile.gameObject.SetActive(true);
            launchCooldown = 2.5f;
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
