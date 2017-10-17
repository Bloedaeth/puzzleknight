﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class MeleeWeapon : MonoBehaviour
{
    public Entity Self;
    public int Damage;
    public bool IsAttacking;

    private Animator anim;
    private new AudioSource audio;
	private AudioClip[] swordWhoosh;
    private AnimatorStateInfo state;
    private AnimatorTransitionInfo trans;

    private int attackStateOneHash;
    private int attackStateTwoHash;
    private int attackStateThreeHash;
    private int attackBossOneHash;
    private int attackBossTwoHash;
    private int attackHash;
    private int blockHash;

    private int transAttackStateOneHash;
    private int transAttackBossOneHash;
    private int transAttackBossTwoHash;
    private int transAttackHash;

    private void Awake()
    {
        anim = Self.GetComponent<Animator>();
        attackStateOneHash = Animator.StringToHash("Base Layer.Light Attacks.Light Attack 1");
        attackStateTwoHash = Animator.StringToHash("Base Layer.Light Attacks.Light Attack 2");
        attackStateThreeHash = Animator.StringToHash("Base Layer.Light Attacks.Light Attack 3");
        attackBossOneHash = Animator.StringToHash("Base Layer.Attack Stage 1");
        attackBossTwoHash = Animator.StringToHash("Base Layer.Attack Stage 2");
        attackHash = Animator.StringToHash("Base Layer.Attack");
        blockHash = Animator.StringToHash("Base Layer.Block");

        transAttackStateOneHash = Animator.StringToHash("Base Layer.Grounded -> Base Layer.Light Attacks.Light Attack 1");
        transAttackBossOneHash = Animator.StringToHash("Base Layer.Idle -> Base Layer.Attack Stage 1");
        transAttackBossTwoHash = Animator.StringToHash("Base Layer.Idle -> Base Layer.Attack Stage 2");
        transAttackHash = Animator.StringToHash("Base Layer.Idle -> Base Layer.Attack");
        
        audio = Self.GetComponent<AudioSource>();
        swordWhoosh = Self.GetComponent<EntitySoundsCommon>().swordSwingSounds;
    }

    /// <summary>Plays a random attack sound from the list of sounds.</summary>
    public void PlaySound()
    {
        if(swordWhoosh.Length > 0)
        {
            audio.clip = swordWhoosh[Random.Range(0, swordWhoosh.Length)];
            audio.Play();
        }
    }

    private void Update()
    {
        state = anim.GetCurrentAnimatorStateInfo(0);
        trans = anim.GetAnimatorTransitionInfo(0);
        IsAttacking =
            state.fullPathHash == attackStateOneHash ||
            state.fullPathHash == attackStateTwoHash ||
            state.fullPathHash == attackStateThreeHash ||
            state.fullPathHash == attackBossOneHash ||
            state.fullPathHash == attackBossTwoHash ||
            state.fullPathHash == attackHash ||

            trans.fullPathHash == transAttackStateOneHash ||
            trans.fullPathHash == transAttackBossOneHash ||
            trans.fullPathHash == transAttackBossTwoHash ||
            trans.fullPathHash == transAttackHash;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!IsAttacking)
            return;

        Entity target = other.gameObject.GetComponent<Entity>();
        Debug.DrawRay(Self.transform.position, Self.transform.forward);

		if (!target)
			return;

        Shield shield = null;
        if(target.transform.CompareTag("Player"))
            shield = target.GetComponent<Player>().Shield;
        else if(target.transform.CompareTag("Enemy"))
        {
            ShieldedEnemy se = target.GetComponent<ShieldedEnemy>();
            if(se)
                shield = se.Shield;
        }

        if(shield != null && (shield.IsBlocking /*|| state.fullPathHash == blockHash*/))
        {
            Vector3 targetDir = target.transform.position - Self.transform.position;
            float angle = Vector3.Angle(-(target.transform.forward), targetDir);

            if(shield.BlockSuccessful(angle))
                shield.Self.Stagger();
            else
                Self.Attack(target, Damage);
        }
        else
        {
            if(target.GetComponent<DeathAnimation>())
                return;

            Self.Attack(target, Damage);
        }
    }
}
