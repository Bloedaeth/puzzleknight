﻿using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class DeathAnimation : MonoBehaviour
{
    private Animator anim;

    private int dieHash;

    private void Awake()
    {
        AICharacterControl ai = GetComponent<AICharacterControl>();
        if(ai)
            ai.enabled = false;

        anim = GetComponent<Animator>();
        anim.SetTrigger("Die");

        dieHash = Animator.StringToHash("Base Layer.Death");

        AudioClip[] possibleDeathSounds = GetComponent<EntitySoundsCommon>().deathSounds;
        AudioClip deathSound = possibleDeathSounds[Random.Range(0, possibleDeathSounds.Length)];

        AudioSource.PlayClipAtPoint(deathSound, transform.position);
    }

    private void Update()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if(state.fullPathHash == dieHash && state.normalizedTime > 1.5f)
        {
            Player player = GetComponent<Player>();
            if(player == null)
                gameObject.SetActive(false);
            else
            {
                transform.position = player.SpawnPoint.position;
                Health hp = GetComponent<Health>();
                hp.enabled = true;
                hp.HealthRemaining = hp.InitialAndMaxHealth;
                anim.SetTrigger("Respawn");
                hp.HealthText.Value = hp.InitialAndMaxHealth;

                Destroy(this);
            }
        }
    }
}
