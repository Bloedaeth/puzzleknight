using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class GruntEnemy : Enemy
{
    private AICharacterControl ai;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private float wanderRadius = 15;
    private float wanderTimer = 5;
    private float timer;

    private float fieldOfView = 80.0f;

    private int attackHash;
    private int blockHash;

    private Vector3 gruntOrigin;

    public Shield shield;

    private void Awake()
    {
        ai = GetComponent<AICharacterControl>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();
        attackHash = Animator.StringToHash("Base Layer.Attack");
        blockHash = Animator.StringToHash("Base Layer.Block");

        agent.stoppingDistance = 1.5f;
        gruntOrigin = transform.position;
    }

    //Mathf.Abs(Vector3.Distance(transform.position, player.position)) < 10f
    //PlayerDetection() == true

    private void Update()
    {
        if(ai.target == null && Mathf.Abs(Vector3.Distance(transform.position, player.position)) < 10f)
            ai.SetTarget(player);

        /*
         * If there is no target,
         * Select a new position using RandomNavSphere and travel there
         * Repeat after every timer until target is found
         */ 
        else if (ai.target == null)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(gruntOrigin, wanderRadius, 1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
        
        /* 
         * Checks if the target is still within line of sight of the enemy
         * If not, remove the target
         */
        if (ai.target != null)
        {
            Vector3 aiPosition = transform.position;
            RaycastHit raycastHit;
            Vector3 rayDirection = ai.target.transform.position - aiPosition;
            Debug.DrawRay(aiPosition, rayDirection);

            if (Physics.Raycast(aiPosition, rayDirection, out raycastHit))
            {
                if (raycastHit.transform.tag != "Player")
                    ai.target = null;
            }
        }

        float dist = Mathf.Abs(Vector3.Distance(transform.position, player.position));
        if(dist < agent.stoppingDistance)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if(state.fullPathHash != attackHash)
            {
                transform.LookAt(player); //AI tends to attack at air because wrong rotation
                animator.SetTrigger("Attack");
            }
            if (shield.IsBlocking == false && state.fullPathHash != blockHash && this.GetComponent<Health>().WasAttackedRecently == true)
            {
                transform.LookAt(player);
                SetBlocking(true);
            }
            else if (shield.IsBlocking == true && this.GetComponent<Health>().WasAttackedRecently == true)
            {
                SetBlocking(false);
            }

            /*
            else if (this.GetComponent <Health>().WasAttackedRecently == false)
            {
                SetBlocking(false);
            }
            */
        }

        AlertOthers();
    }

    private void AlertOthers()
    {
        if (ai.target != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);

            foreach (Collider hit in hitColliders)
            {
                if (hit.tag == "Enemy")
                {
                    hit.GetComponent<GruntEnemy>().ai.SetTarget(player);
                }
            }
        }
    }

    private bool PlayerDetection()
    {
        RaycastHit hit;
        Vector3 rayDirection = ai.target.transform.position - transform.position;

        float distanceToPlayer = Vector3.Distance(transform.position, ai.target.transform.position);

        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {

            if ((hit.transform.tag == ai.target.tag) && (distanceToPlayer <= 10f))
            {
                return true;
            }
        }

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfView)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.transform.tag == ai.target.tag)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    private void SetBlocking(bool value)
    {
        shield.IsBlocking = value;
        animator.SetBool("Block", value);
    }

    private static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, int damage)
    {
        base.Attack(target, damage);
    }
}
