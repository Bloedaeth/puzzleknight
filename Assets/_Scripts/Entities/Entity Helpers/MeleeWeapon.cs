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
    private int attackHash;

    private void Awake()
    {
        anim = Self.GetComponent<Animator>();
        attackStateOneHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 1");
        attackStateTwoHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 2");
        attackStateThreeHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 3");
        attackHash = Animator.StringToHash("Base Layer.Attack");

        audio = GetComponent<AudioSource>();
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
           state.fullPathHash != attackStateThreeHash &&
           state.fullPathHash != attackHash)
            return;

        Entity target = collision.gameObject.GetComponent<Entity>();
        Debug.Log(target);
        if (target)
        {
            Debug.Log("Weappon Hit");
            if (target.GetComponent<DeathAnimation>())
                return;

            Self.Attack(target, Damage);
        }

        Shield shield = target.GetComponent<Shield>();
        //Shield shield = collision.gameObject.GetComponent<Shield>();
        Debug.Log(shield);
        if (shield)
        {
            Debug.Log("Shield == true");
            if(shield.IsBlocking)
            {
                Debug.Log("Shield.IsBloking == true");
                if (!shield.BlockSuccessful())
                {
                    Self.Attack(shield.Self, Damage);
                    Debug.Log("!shield.BlockSuccessful");
                }
                    
            }
            else
            {
                Self.Attack(shield.Self, Damage / 2);
                Debug.Log("else");
            }
                
        }
    }
}
