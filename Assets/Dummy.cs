using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Dummy : MonoBehaviour
{
    private Animator animator;
    private int hitHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitHash = Animator.StringToHash("Base Layer.Dummy Hit");
    }

    private void OnTriggerEnter(Collider other)
    {
        MeleeWeapon weapon = other.GetComponent<MeleeWeapon>();
        if(weapon.IsAttacking && animator.GetCurrentAnimatorStateInfo(0).fullPathHash != hitHash)
            animator.SetTrigger("Attacked");
    }
}
