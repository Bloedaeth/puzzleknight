using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MeleeWeapon : MonoBehaviour
{
    public Entity Self;
    public int Damage;

    private Animator anim;
    private new AudioSource audio;
    private AudioClip[] attackSounds;

    private int attackStateOneHash;
    private int attackStateTwoHash;
    private int attackStateThreeHash;

    private void Awake()
    {
        anim = Self.GetComponent<Animator>();
        attackStateOneHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 1");
        attackStateTwoHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 2");
        attackStateThreeHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 3");

        audio = Self.GetComponent<AudioSource>();
        attackSounds = Self.GetComponent<EntitySoundsCommon>().attackSounds;
    }

    /// <summary>Plays a random attack sound from the list of sounds.</summary>
    public void PlaySound()
    {
        audio.clip = attackSounds[Random.Range(0, attackSounds.Length)];
        audio.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if(state.fullPathHash != attackStateOneHash &&
           state.fullPathHash != attackStateTwoHash &&
           state.fullPathHash != attackStateThreeHash)
            return;

        Entity target = collision.gameObject.GetComponent<Entity>();
        if(target)
        {
            if(target.GetComponent<DeathAnimation>())
                return;

            Self.Attack(target, Damage);
        }

        Shield shield = collision.gameObject.GetComponent<Shield>();
        if(shield)
        {
            if(shield.IsBlocking)
            {
                if(!shield.BlockSuccessful())
                    Self.Attack(shield.Self, Damage / 2);
            }
            else
                Self.Attack(shield.Self, Damage);
        }
    }
}
