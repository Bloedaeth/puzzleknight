using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class DeathAnimation : MonoBehaviour
{
    private Animator anim;

    private int dieHash;

    private void Awake()
    {
        ThirdPersonUserControl uc = GetComponent<ThirdPersonUserControl>();
        if(uc)
            uc.movementActive = false;

        AICharacterControl ai = GetComponent<AICharacterControl>();
        if(ai)
            ai.enabled = false;

		Rigidbody rb = GetComponent<Rigidbody> ();
		if (rb)
			rb.constraints = RigidbodyConstraints.FreezeAll;
		
        anim = GetComponent<Animator>();
        anim.SetTrigger("Die");

        dieHash = Animator.StringToHash("Base Layer.Death");

        AudioClip[] possibleDeathSounds = GetComponent<EntitySoundsCommon>().deathSounds;
        AudioClip deathSound = possibleDeathSounds[Random.Range(0, possibleDeathSounds.Length)];

        AudioSource.PlayClipAtPoint(deathSound, transform.position, PlayPrefs.GameSoundVolume);
    }

    private void Update()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if(state.fullPathHash == dieHash && state.normalizedTime > 1.5f)
        {
            Player player = GetComponent<Player>();
            if(player == null)
            {
                if(GetComponent<BossEnemy>())
                    FindObjectOfType<LevelManager>().LoadNextLevel();
                gameObject.SetActive(false);
            }
            else
            {
                anim.SetTrigger("Respawn");

                Health hp = GetComponent<Health>();
                hp.enabled = true;
                hp.ResetHealth();
                transform.position = player.SpawnPoint.position;

				Rigidbody rb = GetComponent<Rigidbody> ();
				rb.velocity = Vector3.zero;
				rb.constraints = RigidbodyConstraints.None;

                if(player.InBossFight)
                {
                    FindObjectOfType<BossEnemy>().ResetBoss();
                    player.InBossFight = false;
                }

                GetComponent<ThirdPersonUserControl>().movementActive = true;
                Destroy(this);
            }
        }
    }
}
