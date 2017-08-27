using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class BossEnemy : ShieldedEnemy
{
    private int stage = 0;
    public int Stage
    {
        get { return stage; }
        set
        {
            stage = value;
            animator.SetInteger("Stage", stage);
            if(value == 0)
            {
                for(int i = 0; i < pylons.Count; ++i)
                    pylons[i].ResetPylon();
            }
            else
                pylons[stage - 1].SetPylonActive(true);
        }
    }

    private float bossScaleMult = 1f;
    public float BossScaleMult
    {
        get { return bossScaleMult; }
        set
        {
            bossScaleMult = value;
            StartCoroutine(SmoothScale());
        }
    }

    private List<Pylon> pylons;

    private Health hp;
    private AICharacterControl ai;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private GameObject startCollider;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private const float GAME_SCALE_MULT = 10f;
    private int attackHashStage1;
    private int attackHashStage2;

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
        attackHashStage1 = Animator.StringToHash("Base Layer.Attack Stage 1");
        attackHashStage2 = Animator.StringToHash("Base Layer.Attack Stage 2");

        agent.stoppingDistance = 1.5f * GAME_SCALE_MULT;
        startCollider = FindObjectOfType<StartBossFight>().gameObject;
    }

    private void Update()
    {
        if(!hp.IsInvulnerable && transform.localScale.x > originalScale.x)
            hp.IsInvulnerable = true;
        else if(hp.IsInvulnerable && transform.localScale.x <= originalScale.x)
            hp.IsInvulnerable = false;

        if(hp.HealthRemaining <= hp.InitialAndMaxHealth / 2f && Stage != 2)
            Stage = 2;

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

    private IEnumerator SmoothScale()
    {
        Vector3 newScale = originalScale * bossScaleMult;
        float step = 0.05f;

        if(newScale.x > transform.localScale.x)
            while(transform.localScale.x < newScale.x)
            {
                transform.localScale += new Vector3(step, step, step);
                yield return new WaitForFixedUpdate();
            }
        else
            while(transform.localScale.x > newScale.x)
            {
                transform.localScale -= new Vector3(step, step, step);
                yield return new WaitForFixedUpdate();
            }
        yield return null;
    }

    public void ResetBoss()
    {
        Stage = 0;
        ai.target = null;
        transform.position = originalPosition;
        transform.localScale = originalScale;
        hp.HealthRemaining = hp.InitialAndMaxHealth;
        startCollider.SetActive(true);
    }
    
    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, float damage)
    {
        base.Attack(target, damage + 5 * Mathf.RoundToInt(bossScaleMult));
    }
}
