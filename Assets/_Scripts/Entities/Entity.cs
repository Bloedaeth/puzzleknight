using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(EntitySoundsCommon))]
public abstract class Entity : MonoBehaviour
{
    protected Animator animator;
	protected new AudioSource audio;
    protected Rigidbody rb;

	private AudioClip[] attackSounds;

    private int attackStateOneHash;
    private int attackStateTwoHash;
    protected int attackStateThreeHash;
    private int attackBossOneHash;
    private int attackBossTwoHash;
    protected int attackHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
		attackSounds = GetComponent<EntitySoundsCommon>().attackHitSounds;
        rb = GetComponent<Rigidbody>();

        attackStateOneHash = Animator.StringToHash("Base Layer.Light Attacks.Light Attack 1");
        attackStateTwoHash = Animator.StringToHash("Base Layer.Light Attacks.Light Attack 2");
        attackStateThreeHash = Animator.StringToHash("Base Layer.Light Attacks.Light Attack 3");
        attackBossOneHash = Animator.StringToHash("Base Layer.Attack Stage 1");
        attackBossTwoHash = Animator.StringToHash("Base Layer.Attack Stage 2");
        attackHash = Animator.StringToHash("Base Layer.Attack");
    }

    private void Update()
    {
        if(GetComponent<DeathAnimation>())
            return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if(state.fullPathHash != attackStateOneHash &&
           state.fullPathHash != attackStateTwoHash &&
           state.fullPathHash != attackStateThreeHash &&
           state.fullPathHash != attackBossOneHash &&
           state.fullPathHash != attackBossTwoHash &&
           state.fullPathHash != attackHash &&
           rb.constraints == RigidbodyConstraints.FreezeRotation)
            rb.constraints = RigidbodyConstraints.FreezeAll;
        else if(rb.constraints == RigidbodyConstraints.FreezeAll)
            rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public virtual void Attack(Entity target, int damage)
    {
        Debug.Log(name + " attacking " + target.name + " for " + damage + " damage!");
        target.GetComponent<Health>().TakeDamage(damage);
    }

    /// <summary>Triggers the stagger animation of the entity.</summary>
    public virtual void Stagger()
    {
        SetBlock(false);
        animator.SetTrigger("BlockStagger");
        Invoke("SetBlock", animator.GetCurrentAnimatorStateInfo(0).length);
    }

	public void AttackTaunt()
	{
		if(attackSounds.Length > 0)
		{
			audio.clip = attackSounds[Random.Range(0, attackSounds.Length)];
			audio.Play();
		}
	}

    private void SetBlock()
    {
        GetComponentInChildren<Shield>().IsBlocking = true;
    }

    private void SetBlock(bool state)
    {
        GetComponentInChildren<Shield>().IsBlocking = state;
    }
}
