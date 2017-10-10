using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class BossEnemy : Enemy
{
    private int stage = 0;
    public int Stage
    {
        get { return stage; }
        set
        {
            stage = value;
            if(stage == 0)
            {
                for(int i = 0; i < pylons.Count; ++i)
                    pylons[i].ResetPylon();
            }
            else
                pylons[stage - 1].SetPylonActive(true);
        }
    }

    private float bossScaleMult = 1f;

    private List<Pylon> pylons;

    //private Animator animator;
    private Health hp;
    private AICharacterControl ai;
    private NavMeshAgent agent;
    private Transform player;
    [SerializeField] private ParticleSystem ps;
    private GameObject startCollider;
    [SerializeField] private ParticleSystem stompParticles;
    private BossSounds sounds;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private int attackHashStage1;
    private int attackHashStage2;

    private bool scaling;

    private const float EXPANSION_RATE_MULT = 1f;

	private float particleTime;
	private float particleRate = 2f;

    private void Awake()
    {
        pylons = FindObjectsOfType<Pylon>().OrderBy(p => p.ID).ToList();

        originalScale = transform.localScale;
        originalPosition = transform.position;

        hp = GetComponent<Health>();
        ai = GetComponent<AICharacterControl>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();
        sounds = GetComponent<BossSounds>();

        attackHashStage1 = Animator.StringToHash("Base Layer.Attack Stage 1");
        attackHashStage2 = Animator.StringToHash("Base Layer.Attack Stage 2");

        startCollider = FindObjectOfType<StartBossFight>().gameObject;
    }

    private void Update()
    {
        SetHurtSounds();

        agent.stoppingDistance = 1.5f + bossScaleMult - 1;
        //if(bossScaleMult >= 2f)
        //    animator.SetInteger("Stage", 2);
        //else
        	animator.SetInteger("Stage", 1);
		
        if(!hp.IsInvulnerable && transform.localScale.x > originalScale.x)
        {
            ps.Play();
            hp.IsInvulnerable = true;
        }
        else if(hp.IsInvulnerable && transform.localScale.x <= originalScale.x)
        {
            ps.Stop();
            hp.IsInvulnerable = false;
        }

        if(hp.HealthRemaining <= hp.InitialAndMaxHealth / 2f && Stage != 2)
            Stage = 2;
        
        float dist = Mathf.Abs(Vector3.Distance(transform.position, player.position));
        if(dist <= agent.stoppingDistance)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
			if(state.fullPathHash != attackHashStage1 && state.fullPathHash != attackHashStage2  && Time.time > particleTime)
            {
                Vector3 look = player.position - transform.position;
                look.y = 0;
                transform.rotation = Quaternion.LookRotation(look);
                animator.SetTrigger("Attack");
				if(animator.GetInteger ("Stage") == 2)
				{
					particleTime = Time.time + particleRate;

					stompParticles.transform.position = transform.position + transform.forward;
					stompParticles.Play();
				}
            }
        }

		ScaleBoss (); // Use this instead ~ Steve
    }

    private void SetHurtSounds()
    {
        if(bossScaleMult <= 1f)
            sounds.hurtSounds = sounds.hurtSize1;
        else if(bossScaleMult <= 2f)
            sounds.hurtSounds = sounds.hurtSize2;
        else
            sounds.hurtSounds = sounds.hurtSize3;
    }

	private void ScaleBoss() { // Here is the method I'm using
		bossScaleMult = 1f;

		for (int i = 0; i < pylons.Count; i++) {
			bossScaleMult += pylons [i].pylonScaleModifier;
		}

		transform.localScale = originalScale * bossScaleMult;
	}

    private IEnumerator SmoothScale(Vector3 start, Vector3 end, Pylon pylon)
    {
        float step = 0;
        while(step < 1f)
        {
            step += 1 / pylon.RAISE_LOWER_TIME * Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, end, step);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    /// <summary>Scales the boss by a fixed amount over time.</summary>
    /// <param name="scaleIncrease">The increase in the boss' scale.</param>
    /// <param name="pylon">The pylon doing the scaling, determines the scale time.</param>
    public void ScaleOverTime(float scaleIncrease, Pylon pylon)
    {
        Vector3 start = originalScale * bossScaleMult;
        bossScaleMult += scaleIncrease;
        Vector3 end = originalScale * bossScaleMult;
        StartCoroutine(SmoothScale(start, end, pylon));
    }

    /// <summary>Resets the boss to its original state from before the boss fight started.</summary>
    public void ResetBoss()
    {
        Stage = 0;
        transform.localScale = originalScale;
        transform.position = originalPosition;

        ai.SetTarget(null);
        ai.agent.SetDestination(originalPosition);
        
        hp.ResetHealth();
        hp.HealthBar.transform.parent.gameObject.SetActive(false);

        startCollider.SetActive(true);
    }
    
    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, int damage)
    {
        base.Attack(target, damage + 5 * Mathf.RoundToInt(bossScaleMult));
    }
}
