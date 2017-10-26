using GameLogging;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class GruntEnemy : ShieldedEnemy
{
    private AICharacterControl ai;
    private NavMeshAgent agent;
    private Transform player;
    private AnimatorStateInfo state;
	private new AudioSource audioS;

    private float wanderRadius = 15;
    private float wanderTimer = 5;
    private float timer;

    private int testNumber;
    //private float testStopDistance = 3.5f;

    private float attackTimer = 1.5f;
    private float timer2;
    
    //private const float SCALE_MULT = 10f;
    private const float MIN_DIST = 10f;

    //private int attackHash;
    //private int blockHash;

    private Vector3 gruntOrigin;

    //private Animator animator;
    //private Rigidbody rb;

	private MeleeWeapon club;

    private void Awake()
    {
		audioS = GetComponent<AudioSource> ();
		esc = GetComponent<EntitySoundsCommon> ();
        ai = GetComponent<AICharacterControl>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();
        attackHash = Animator.StringToHash("Base Layer.Attack");
        //blockHash = Animator.StringToHash("Base Layer.Block");
		deathHash = Animator.StringToHash("Base Layer.Death");

		club = GetComponentInChildren<MeleeWeapon> ();
        agent.stoppingDistance = 1.5f;// * SCALE_MULT;
        gruntOrigin = transform.position;
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
            animator.speed = 1f;
            agent.speed = 1f;
        }
        
        float dist = Mathf.Abs(Vector3.Distance(transform.position, player.position));
        if(ai.target == null && dist < MIN_DIST/* * SCALE_MULT*/)
        {
            BuildDebug.Log(name + " targeting player.");
            ai.SetTarget(player);
        }
        /*
         * If there is no target,
         * Select a new position using RandomNavSphere and travel there
         * Repeat after every timer until target is found
         */
        else if(ai.target == null)
        {
            timer += Time.deltaTime;

            if(timer >= wanderTimer)
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
            //Make it a bit above the ground, so collisions with ground objects don't occur
            Vector3 aiPosition = transform.position + Vector3.up;
            RaycastHit raycastHit;
            Vector3 rayDirection = (ai.target.transform.position + Vector3.up) - aiPosition;

            if(Physics.Raycast(aiPosition, rayDirection, out raycastHit))
            {
                if (raycastHit.transform.tag != "Player")
                    ai.target = null;
            }
        }

        state = animator.GetCurrentAnimatorStateInfo(0);

		if (state.fullPathHash == deathHash) {
			GetComponent<Collider> ().enabled = false;
		}

        if (dist < agent.stoppingDistance + 0.5)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);

            foreach (Collider hit in hitColliders)
            {
                if (hit.transform.tag == "Enemy")
                {
                    if (Vector3.Distance(hit.transform.position, transform.position) < 5)
                    {
                        float step = -0.1f;
                        transform.position = Vector3.MoveTowards(transform.position, hit.transform.position, step);
                    }
                }

            }                
            
            state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.fullPathHash != attackHash && !GetComponent<DeathAnimation>())
            {
                timer2 += Time.deltaTime;
                Vector3 look = player.position - transform.position;
                look.y = 0;
                transform.rotation = Quaternion.LookRotation(look);
                if (timer2 >= attackTimer)
                {
                    BuildDebug.Log(name + " attacking player.");
                    animator.SetTrigger("Attack");
                    timer2 = 0;
                }
            }
        }

        //state = animator.GetCurrentAnimatorStateInfo(0);
        //if(Shield.IsBlocking == false && state.fullPathHash != blockHash && this.GetComponent<Health>().timeSinceDamageTaken < 1)
        //{
        //    transform.LookAt(player);
        //    SetBlocking(true);
        //}
        //else if(Shield.IsBlocking == true && this.GetComponent<Health>().timeSinceDamageTaken > 1)
        //{
        //    SetBlocking(false);
        //}

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
                    BuildDebug.Log(name + " alerting " + hit.name + " to player presence!");
                    hit.GetComponent<GruntEnemy>().ai.SetTarget(player);
                }
            }
        }
    }

    private void SetBlocking(bool value)
    {
        BuildDebug.Log(name + " blocking: " + value);
        Shield.IsBlocking = value;
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

	public void SwingPlaySound() {
		club.PlaySound ();
	}

	void AttackPlaySound() {
		if (esc.attackHitSounds.Length > 0) {
			audioS.clip = esc.attackHitSounds [Random.Range (0, esc.attackHitSounds.Length)];
			audioS.Play ();
		}
	}

    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, int damage)
    {
		if(!(target is Player))
			return;

		if (!audioS.isPlaying) {
			AttackPlaySound ();
		}
        base.Attack(target, damage);
    }
    /*
 *         if (dist < agent.stoppingDistance)
        {
            if (GetComponent<Player>().attackerList.Length == 2 && Random.Range(0, 100) < 50)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);

                foreach (Collider hit in hitColliders)
                {
                    for (int i = 0; i < GetComponent<Player>().attackerList.Length; i++)
                    {
                        if (hit.gameObject == GetComponent<Player>().attackerList[i])
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < 5)
                            {
                                rb.AddForce(transform.right);
                            }
                        }
                    }
                }                
            }
            else if (GetComponent<Player>().attackerList.Length == 2 && Random.Range(0, 100) > 51)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);

                foreach (Collider hit in hitColliders)
                {
                    for (int i = 0; i < GetComponent<Player>().attackerList.Length; i++)
                    {
                        if (hit.gameObject == GetComponent<Player>().attackerList[i])
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < 5)
                            {
                                rb.AddForce(-(transform.right));
                            }
                        }
                    }
                }  
            }
            else if (GetComponent<Player>().attackerList.Length < 2)
            {
                GetComponent<Player>().attackerList[GetComponent<Player>().attackerList.Length] = this.gameObject;
            }
 */
}



