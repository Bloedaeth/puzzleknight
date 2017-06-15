﻿using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class GruntEnemy : Enemy
{
    private AICharacterControl ai;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private int attackHash;

    private void Awake()
    {
        ai = GetComponent<AICharacterControl>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();
        attackHash = Animator.StringToHash("Base Layer.Attack");

        agent.stoppingDistance = 1.5f;
    }

    private void Update()
    {
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

    public override void Attack(Entity target, int damage)
    {
        base.Attack(target, damage);
    }
}
