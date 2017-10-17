using GameLogging;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Dummy : MonoBehaviour
{
    private Animator animator;
    private new AudioSource audio;
    private int hitHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        hitHash = Animator.StringToHash("Base Layer.Dummy Hit");
    }

    private void OnTriggerEnter(Collider other)
    {
        MeleeWeapon weapon = other.GetComponent<MeleeWeapon>();
        if(weapon.IsAttacking && animator.GetCurrentAnimatorStateInfo(0).fullPathHash != hitHash)
        {
            BuildDebug.Log(name + " was attacked.");
            animator.SetTrigger("Attacked");
            audio.Play();
        }
    }
}
