using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MeleeWeapon : MonoBehaviour
{
    public Entity Self;
    Shield shield;
    public int Damage;

    private Animator anim;
    private new AudioSource audio;
    private AudioClip[] attackSounds;

    private int attackStateOneHash;
    private int attackStateTwoHash;
    private int attackStateThreeHash;
    private int attackBossOneHash;
    private int attackBossTwoHash;
    private int attackHash;

    private void Awake()
    {
        anim = Self.GetComponent<Animator>();
        attackStateOneHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 1");
        attackStateTwoHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 2");
        attackStateThreeHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 3");
        attackBossOneHash = Animator.StringToHash("Base Layer.Attack Stage 1");
        attackBossTwoHash = Animator.StringToHash("Base Layer.Attack Stage 2");
        attackHash = Animator.StringToHash("Base Layer.Attack");

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
           state.fullPathHash != attackStateThreeHash &&
           state.fullPathHash != attackBossOneHash &&
           state.fullPathHash != attackBossTwoHash &&
           state.fullPathHash != attackHash)
            return;

        Entity target = collision.gameObject.GetComponent<Entity>();
        Debug.DrawRay(Self.transform.position, Self.transform.forward);

        if (target.transform.tag == "Player")
            shield = target.GetComponent<Player>().shield;
        else if (target.transform.tag == "Enemy")
            shield = target.GetComponent<ShieldedEnemy>().Shield;
        
        if (shield.IsBlocking == true)
        {
            Vector3 targetDir = target.transform.position - Self.transform.position;
            float angle = Vector3.Angle(-(target.transform.forward), targetDir);

            if ((angle < 90.0f) && shield.BlockSuccessful())
                Self.Attack(target, 5);
            else
                Self.Attack(target, Damage);
        }
        else
        {
            if (target.GetComponent<DeathAnimation>())
                return;

            Self.Attack(target, Damage);
        }
    }
}
