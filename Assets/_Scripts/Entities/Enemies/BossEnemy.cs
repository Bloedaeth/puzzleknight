using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using GameLogging;

public class BossEnemy : Enemy
{
    private int stage = 0;
    public int Stage
    {
        get { return stage; }
        set
        {
            stage = value;
            BuildDebug.Log("Setting boss stage to: " + stage);
            if(stage == 0)
            {
                BuildDebug.Log("Restting pylons");
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
    private ParticleSystem[] invulnurableParticleSystems;
    private GameObject startCollider;
    [SerializeField] private ParticleSystem stompParticles;
    private BossSounds sounds;
	private AudioSource aS;

    private Vector3 originalScale;
	private Vector3[] particleSystemOriginalScale;
    private Vector3 originalPosition;
	private float windowOfOpportunity = 1.1f;

    private int attackHashStage1;
    private int attackHashStage2;

    private bool scaling;

    private const float EXPANSION_RATE_MULT = 1f;

	private float particleTime;
	private float particleRate = 2f;

	private MeleeWeapon club;

	bool attacking = false;

	private void Awake()
    {

		aS = GetComponent<AudioSource> ();
		club = GetComponentInChildren<MeleeWeapon> ();
		esc = GetComponent<EntitySoundsCommon> ();

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

		invulnurableParticleSystems = new ParticleSystem[GetComponentsInChildren<ParticleSystem> ().Length - 1];
		particleSystemOriginalScale = new Vector3[invulnurableParticleSystems.Length];

		int j = 0;

		for (int i = 0; i < GetComponentsInChildren<ParticleSystem> ().Length; i++) {
			if (GetComponentsInChildren<ParticleSystem> () [i].CompareTag("BossInvulnerabilityParticles")) {
				invulnurableParticleSystems [j] = GetComponentsInChildren<ParticleSystem> () [i];
				particleSystemOriginalScale [j] = invulnurableParticleSystems [j].shape.box;

				invulnurableParticleSystems [j].Stop ();
				j++;
			}
		}
    }

    private void Update()
    {
        SetHurtSounds();
        
        if (ai.target != player && ai.target != null)
            ai.SetTarget(null);

        agent.stoppingDistance = 1.5f + bossScaleMult - 1;
        //if(bossScaleMult >= 2f)
        //    animator.SetInteger("Stage", 2);
        //else
        if(animator.GetInteger("Stage") != 1)
            animator.SetInteger("Stage", 1);
		
		if(!hp.IsInvulnerable && transform.localScale.x > originalScale.x * windowOfOpportunity)
        {
            BuildDebug.Log("Setting boss to be invulnerable");
            foreach (ParticleSystem ps in invulnurableParticleSystems) {
				ps.Play ();
			}
            hp.IsInvulnerable = true;
        }
		else if(hp.IsInvulnerable && transform.localScale.x <= originalScale.x * windowOfOpportunity)
        {
            BuildDebug.Log("Setting boss to be vulnerable to attacks");
            foreach (ParticleSystem ps in invulnurableParticleSystems) {
				ps.Stop ();
			}
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
				if(animator.GetInteger("Stage") == 2)
				{
					particleTime = Time.time + particleRate;

					stompParticles.transform.position = transform.position + transform.forward;
					stompParticles.Play();
				}
            }
        }

        if(hp.HealthRemaining > 0)
            ScaleBoss();
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

	private void ScaleBoss() {
		bossScaleMult = 1f;

		for (int i = 0; i < pylons.Count; i++) {
			bossScaleMult += pylons [i].pylonScaleModifier;
		}

		ParticleSystem.ShapeModule s;
		for (int i = 0; i < invulnurableParticleSystems.Length; i++) {
			s = invulnurableParticleSystems [i].shape;
			s.box = particleSystemOriginalScale [i] * bossScaleMult;
		}
		transform.localScale = originalScale * bossScaleMult;
	}

	public void PlaySwingSound() {
		if (!ai.target) {
			return;
		}

		if (bossScaleMult >= 1f) {
			club.PlaySound (sounds.bigSwordSwingSounds [Random.Range (0, sounds.bigSwordSwingSounds.Length)]);
		} else {
			club.PlaySound ();
		}
	}

	public void PlayAttackHitSound() {

		if (esc.attackHitSounds.Length > 0) {
			aS.clip = esc.attackHitSounds [Random.Range (0, esc.attackHitSounds.Length)];
			aS.Play ();
		}

	}

    //private IEnumerator SmoothScale(Vector3 start, Vector3 end, Pylon pylon)
    //{
    //    float step = 0;
    //    while(step < 1f)
    //    {
    //        step += 1 / pylon.RAISE_LOWER_TIME * Time.deltaTime;
    //        transform.localScale = Vector3.Lerp(start, end, step);
    //        yield return new WaitForFixedUpdate();
    //    }
    //    yield return null;
    //}

    ///// <summary>Scales the boss by a fixed amount over time.</summary>
    ///// <param name="scaleIncrease">The increase in the boss' scale.</param>
    ///// <param name="pylon">The pylon doing the scaling, determines the scale time.</param>
    //public void ScaleOverTime(float scaleIncrease, Pylon pylon)
    //{
    //    Vector3 start = originalScale * bossScaleMult;
    //    bossScaleMult += scaleIncrease;
    //    Vector3 end = originalScale * bossScaleMult;
    //    StartCoroutine(SmoothScale(start, end, pylon));
    //}

    /// <summary>Resets the boss to its original state from before the boss fight started.</summary>
    public void ResetBoss()
    {
        BuildDebug.Log("Resetting the boss fight");
        Stage = 0;
        transform.localScale = originalScale;
        transform.position = originalPosition;

        ai.SetTarget(null);
        ai.agent.SetDestination(originalPosition);
        
        hp.ResetHealth();
        hp.HealthBar.transform.parent.gameObject.SetActive(false);

        startCollider.SetActive(true);
    }
    
	public void Attacking(int isAttacking) {
		if (isAttacking == 0) {
			attacking = false;
		} else {
			attacking = true;
		}
	}

    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, int damage)
    {
		if (!(target is Player) || !attacking) {
			return;
		}

		attacking = false;

		PlayAttackHitSound ();

        base.Attack(target, damage + 5 * Mathf.RoundToInt(bossScaleMult));
    }
}
